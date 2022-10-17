using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimallAPI_rest;
using minimallAPI_rest.modelos;
using minimallAPI_rest.Properties.modelos;
using System.Linq;
using System.Text.Json.Serialization;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(option => 
{

    option.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("https://sentidos.vercel.app", "http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
      
    });

});
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
//Menu_acantidad[] menussda = new Menu_acantidad[100];
//menussda[0] = new Menu_acantidad() { id_men = Guid.Parse("0f860902-f798-4c59-a513-1ebd0b39bbe8"), cantidad = 3 };
//para_pedido pedido30 = new para_pedido();
//pedido30.nombre = "maxi";
//pedido30.apellido = "Ruiz Diaz";
//pedido30.dni = "4452455";
//pedido30.hora="20:00";
//pedido30.fecha = "20-10-2022";
//pedido30.menu_s = menussda;




builder.Services.AddSqlServer<RestauranteContext>(builder.Configuration.GetConnectionString("bd1"));

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);




app.MapGet("/", () => "Hello World!");
app.MapGet("/dbconexion", async ([FromServices] RestauranteContext dbContext) =>
{
    dbContext.Database.EnsureCreated();
    return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory());

});
app.MapPost("/api/usuario_create", async ([FromServices] RestauranteContext dbContext, [FromBody] Usuario Usuario_nuevo) => 

{
    string password_hash = Comprobaciones.GetSHA256(Usuario_nuevo.password).ToString();
    bool nicknamerepetido = false;
    bool email_repetido = false;
    bool dni_repetido = false;
    foreach (Usuario usuario in dbContext.usuarios ) {
        if ((string.Compare(usuario.email, Usuario_nuevo.email))== 0) {
            email_repetido = true;
        }
        if ((string.Compare(usuario.nickname, Usuario_nuevo.nickname)) == 0)
        {
            nicknamerepetido = true;
        }
        if ((string.Compare(usuario.dni, Usuario_nuevo.dni)) == 0)
        {
            dni_repetido = true;
        }
    }

   
    if (nicknamerepetido == true && email_repetido == true && dni_repetido==true)
    {

        Results.NotFound("el email, nickname y dni ya estan utilizados");

    }
    else if (email_repetido == true && nicknamerepetido == true && dni_repetido == false)
    {

        return Results.NotFound("El email y nickname ya esta registrado");
    }


    else if (nicknamerepetido == true && email_repetido == false&& dni_repetido==false)
    {

        return Results.NotFound("El nickname ya estan siendo utilizado");


    }
    else if (email_repetido == true && nicknamerepetido== false&& dni_repetido == false)
    {

        return Results.NotFound("El email ya esta registrado");
    }
    else if (email_repetido == true && nicknamerepetido == false && dni_repetido == true)
    {

        return Results.NotFound("El email y dni ya esta registrado");
    }
    else if (email_repetido == false&& nicknamerepetido == true && dni_repetido == true)
    {

        return Results.NotFound("El nickname y dni ya esta registrado");
    }
    else if (email_repetido == false && nicknamerepetido == false && dni_repetido == true)
    {

        return Results.NotFound("El dni ya esta registrado");
    }

  else
    {

        Usuario_nuevo.password = password_hash;
        Usuario_nuevo.id_usuario = Guid.NewGuid();
        await dbContext.AddAsync(Usuario_nuevo);

        await dbContext.SaveChangesAsync();
        return Results.Ok(Usuario_nuevo);
    }
    return Results.NotFound();


});
app.MapPut("/api/login", async([FromServices] RestauranteContext dbcontext, [FromBody] Usuario usuario_nuevo) => {
bool cuentacorrecta = false;
   usuario_nuevo.password = Comprobaciones.GetSHA256(usuario_nuevo.password);

    foreach (Usuario usuario in dbcontext.usuarios)
    {
        if( string.Compare(usuario.password,usuario_nuevo.password) == 0 && string.Compare(usuario.email, usuario_nuevo.email)==0)
        {
          usuario_nuevo.id_usuario=usuario.id_usuario;
            cuentacorrecta = true;
            break;

        }



    }
    if (cuentacorrecta == true) {
        var usuario = dbcontext.usuarios.Find(usuario_nuevo.id_usuario);
        return Results.Ok(usuario);
    
    }
    return Results.NotFound();




});








app.MapGet("/api/reservas/reserva/{fecha}/{hora}", async ([FromServices] RestauranteContext dbContext, [FromRoute] string fecha, [FromRoute] string hora) => {

    if (comprobacion_existen_reser(fecha, dbContext) == false)
    {

      creacionesde_mesa(fecha, dbContext);
     
      await dbContext.SaveChangesAsync();

    };

   



    return Results.Ok(dbContext.reserva.Include(p => p.MESA).Where(p => string.Compare(p.fecha_de_reseva, fecha) == 0 && string.Compare(p.hora_reserva,hora) ==0 && p.reservado == false));


});

app.MapGet("/api/reservas/{id_usuario}", async ([FromServices] RestauranteContext dbContext, [FromRoute] string id_usuario ) => {
    Guid id_usu = Guid.Parse(id_usuario);

    return Results.Ok(dbContext.reserva.Include(p => p.MESA).Where(p => p.reservado == true && p.ID_usuario==id_usu));




});

app.MapPut("api/cancelar_reserva/{id_reserva}",async([FromServices] RestauranteContext dbContext, [FromRoute] string id_reserva) => 
{
    Guid reserva_id= Guid.Parse(id_reserva);
    var reserva_actual = dbContext.reserva.Find(reserva_id);
    if (reserva_actual!= null) {
        reserva_actual.cantidad_personas = 0;
        reserva_actual.ID_usuario = Guid.Parse("38043687-c388-4a59-ad74-766fd2677441");
        reserva_actual.reservado = false;
        await dbContext.SaveChangesAsync();
        return Results.Ok();





    }
    return Results.NotFound(); 








} ); 

app.MapDelete("/api/eliminar/{id_eliminar}", async ([FromServices] RestauranteContext dbcontext,[FromRoute] Guid id_eliminar) =>
{

    foreach (Reserva resrva_eliminar in dbcontext.reserva) {

        if (resrva_eliminar.ID_usuario == id_eliminar) {

            dbcontext.reserva.Remove(resrva_eliminar);
        
        }
    
    
    }
    await dbcontext.SaveChangesAsync();
    return Results.Ok(dbcontext.reserva);




});


app.MapPut("/api/para_reservar/{id_usuario}/{id_mesa}/{cantidad_personas}/{fecha}/{hora}", async ([FromServices] RestauranteContext dbContext, [FromRoute] string id_mesa, [FromRoute] string id_usuario, [FromRoute] string cantidad_personas,[FromRoute] string fecha,[FromRoute] string hora) =>
{
    bool reserva_registrada = false;
    string id_reser = "";
    Guid mesa_id = Guid.Parse(id_mesa);
    Guid usuario = Guid.Parse(id_usuario);
    if (Comprobaciones.comprobacion_reser_posible(fecha) <= 30)
    {
        foreach (Reserva reservas in dbContext.reserva)
        {

            if (reservas.id_MESA == mesa_id && string.Compare(reservas.fecha_de_reseva, fecha) == 0 && reservas.reservado == false && string.Compare(reservas.hora_reserva, hora)==0)
            {
                id_reser = reservas.id_reservar.ToString();
                dbContext.Remove(reservas);
                dbContext.Add(new Reserva() { reservado = true, fecha_de_reseva = fecha, id_MESA = mesa_id, ID_usuario = usuario, cantidad_personas = int.Parse(cantidad_personas), id_reservar = Guid.Parse(id_reser) , hora_reserva=hora}); 
                reserva_registrada = true;

                break;
            }



        }
    }
    else
        return Results.Ok("solo se puede reserva 30 dias antes");
  
    

        await dbContext.SaveChangesAsync();
    if (reserva_registrada==true) {
        return Results.Ok(dbContext.reserva.Include(p => p.MESA).Include(p => p.usuario).Where(p => p.id_MESA == mesa_id && p.ID_usuario == usuario && string.Compare(p.fecha_de_reseva, fecha) == 0 && string.Compare(p.hora_reserva,hora)== 0));

    } else
        return Results.NotFound();




});
app.MapDelete("/api/eliminar_mesa/{id_eliminar}", async ([FromServices] RestauranteContext dbcontext, [FromRoute] Guid id_eliminar) =>
{

    foreach (Reserva resrva_eliminar in dbcontext.reserva)
    {

        if (resrva_eliminar.id_MESA== id_eliminar)
        {

            dbcontext.Remove(resrva_eliminar);

        }


    }
    await dbcontext.SaveChangesAsync();
    return Results.Ok();




});
app.MapDelete("/api/eliminar_usuario", async ([FromServices] RestauranteContext dbcontext) =>
{



    foreach (Reserva resrva_eliminar in dbcontext.reserva)
    {

        dbcontext.Remove(resrva_eliminar);


    }
    await dbcontext.SaveChangesAsync();


});


app.MapPut("api/confirmar_pago/{id_reserva}", async ([FromServices] RestauranteContext dbcontext,  [FromRoute] string id_reserva) => 
{
    Guid id = Guid.Parse(id_reserva);
    var reserva_actual = dbcontext.reserva.Find(id);

    if (reserva_actual != null) 
    {
        reserva_actual.confirmo_reserva = true;
        await dbcontext.SaveChangesAsync();
        return Results.Ok();




    }
    else 
        
        return Results.NotFound(); 
    

   

});
app.MapGet("api/get/menu", async([FromServices] RestauranteContext dbcontext) => {




    return Results.Ok(dbcontext.Menus);




});
app.MapGet("api/get/metodo_pago", async ([FromServices] RestauranteContext dbcontext) => {




    return Results.Ok(dbcontext.metodo_Pagos);




});
app.MapGet("api/get/clientes", async ([FromServices] RestauranteContext dbcontext) => {





    return Results.Ok(dbcontext.clientes);




});
app.MapGet("api/get/mesas_reservadas/{fecha}/{hora}", async ([FromServices] RestauranteContext dbcontext, [FromRoute] string fecha,[FromRoute] string hora  ) => {



    return Results.Ok(dbcontext.reserva.Include(p => p.MESA).Where(p => string.Compare(p.fecha_de_reseva, fecha) == 0 && string.Compare(p.hora_reserva, hora) == 0 && p.reservado == true));



});
app.MapPost("api/create_Empleado/{modo_rol}", async ([FromServices] RestauranteContext dbcontex, [FromBody] Empleado nuevo_empleado, [FromRoute] string modo_rol) => 

{

    bool nicknamerepetido = false;
    bool email_repetido = false;

    foreach (Empleado empleado in dbcontex.empleados)
    {
        if ((string.Compare(empleado.email_empleado, nuevo_empleado.email_empleado)) == 0)
        {
            email_repetido = true;
        }
        if ((string.Compare(empleado.nickname_empleado, nuevo_empleado.nickname_empleado)) == 0)
        {
            nicknamerepetido = true;
        }
    
    }


    if (nicknamerepetido == true && email_repetido == true )
    {

        Results.NotFound("el email, nickname ya estan utilizados");

    }
    else if (email_repetido == true && nicknamerepetido == false)
    {

        return Results.NotFound("El email ya esta registrado");
    }


    else if (nicknamerepetido == true && email_repetido == false )
    {

        return Results.NotFound("El nickname ya estan siendo utilizado");


    }
    else
    {

        nuevo_empleado.password_empleado = Comprobaciones.GetSHA256(nuevo_empleado.password_empleado);
        nuevo_empleado.rol =int.Parse(modo_rol);
        nuevo_empleado.id_empleado = Guid.NewGuid();
        await dbcontex.AddAsync(nuevo_empleado);

        await dbcontex.SaveChangesAsync();
        return Results.Ok(nuevo_empleado);
    }
    return Results.NotFound();




});


app.MapPut("/api/login_empleado", async ([FromServices] RestauranteContext dbcontext, [FromBody] Empleado empleado_nuevo) => {
    bool cuentacorrecta = false;
    empleado_nuevo.password_empleado = Comprobaciones.GetSHA256(empleado_nuevo.password_empleado);

    foreach (Empleado usuario in dbcontext.empleados)
    {
        if (string.Compare(usuario.password_empleado, empleado_nuevo.password_empleado) == 0 && string.Compare(usuario.email_empleado, empleado_nuevo.email_empleado) == 0)
        {
            empleado_nuevo.id_empleado= usuario.id_empleado;
            cuentacorrecta = true;
            break;

        }



    }
    if (cuentacorrecta == true)
    {
        var usuario = dbcontext.empleados.Find(empleado_nuevo.id_empleado);
        return Results.Ok(usuario);

    }
    return Results.NotFound();






});
app.MapPost("api/create_Pedido", async ([FromServices] RestauranteContext dbcontext, [FromBody] para_pedido pedido) =>
{
    
    var cliente = dbcontext.clientes.Find(pedido.dni);
    if (cliente == null)
    {
        Cliente cliente_nuevo = new Cliente();
        cliente_nuevo.dni = pedido.dni;
        cliente_nuevo.nombre_cliente = pedido.nombre;
        cliente_nuevo.apellido_cliente = pedido.apellido;
        dbcontext.AddAsync(cliente_nuevo);


    };

    Pedido pedido_usuario = new Pedido();
    pedido_usuario.fecha_pedido = pedido.fecha;
    pedido_usuario.hora_pedido = pedido.hora;
    pedido_usuario.id_dni_cliente = pedido.dni;
    pedido_usuario.id_mesaa = Guid.Parse(pedido.id_mesa);
    pedido_usuario.id_Pedido = Guid.NewGuid();
    pedido_usuario.metodo_pago_pedido = 1;
   
   


    dbcontext.AddAsync(pedido_usuario);
  
  


    creacion_De_menus(pedido.menu_s,pedido_usuario.id_Pedido, dbcontext);

    await dbcontext.SaveChangesAsync();
    return Results.Ok(dbcontext.pedidos.Include(p=> p.cliente).Include(p=> p.mesa_pedido).Where(p=>p.id_Pedido == pedido_usuario.id_Pedido));
});

app.MapGet("probando_pedidos", async([FromServices] RestauranteContext dbcontext) => 
{


    return Results.Ok(dbcontext.pedidos.Include(p=>p.mesa_pedido).Include(p=> p.cliente).Include(p=>p.metodo));
});



void creacionesde_mesa(string fecha, RestauranteContext dbContext)
{

    foreach (Mesa mesa in dbContext.mesas)
    {
        dbContext.Add(new Reserva() { reservado = false, fecha_de_reseva = fecha, id_MESA = mesa.id_mesa, ID_usuario = Guid.Parse("38043687-c388-4a59-ad74-766fd2677441"), cantidad_personas = 0, id_reservar = Guid.NewGuid(), hora_reserva = "08:00" });
        dbContext.Add(new Reserva() { reservado = false, fecha_de_reseva = fecha, id_MESA = mesa.id_mesa, ID_usuario = Guid.Parse("38043687-c388-4a59-ad74-766fd2677441"), cantidad_personas = 0, id_reservar = Guid.NewGuid(), hora_reserva = "10:00" });
        dbContext.Add(new Reserva() { reservado = false, fecha_de_reseva = fecha, id_MESA = mesa.id_mesa, ID_usuario = Guid.Parse("38043687-c388-4a59-ad74-766fd2677441"), cantidad_personas = 0, id_reservar = Guid.NewGuid(), hora_reserva = "12:00" });
        dbContext.Add(new Reserva() { reservado = false, fecha_de_reseva = fecha, id_MESA = mesa.id_mesa, ID_usuario = Guid.Parse("38043687-c388-4a59-ad74-766fd2677441"), cantidad_personas = 0, id_reservar = Guid.NewGuid(), hora_reserva = "14:00" });
        dbContext.Add(new Reserva() { reservado = false, fecha_de_reseva = fecha, id_MESA = mesa.id_mesa, ID_usuario = Guid.Parse("38043687-c388-4a59-ad74-766fd2677441"), cantidad_personas = 0, id_reservar = Guid.NewGuid(), hora_reserva = "17:00" });
        dbContext.Add(new Reserva() { reservado = false, fecha_de_reseva = fecha, id_MESA = mesa.id_mesa, ID_usuario = Guid.Parse("38043687-c388-4a59-ad74-766fd2677441"), cantidad_personas = 0, id_reservar = Guid.NewGuid(), hora_reserva = "20:00" });
        dbContext.Add(new Reserva() { reservado = false, fecha_de_reseva = fecha, id_MESA = mesa.id_mesa, ID_usuario = Guid.Parse("38043687-c388-4a59-ad74-766fd2677441"), cantidad_personas = 0, id_reservar = Guid.NewGuid(), hora_reserva = "22:00" });




    }





}
void creacion_De_menus(Menu_acantidad[] cantidad_menu, Guid id_pedido, RestauranteContext db_context) {

    for (int i = 0; i < cantidad_menu.Length; i++) {
        if (cantidad_menu[i] != null) {
            for (int o = 0; o < cantidad_menu[i].cantidad; o++)
            {
                Pedido_Menu nuevo_pedido = new Pedido_Menu();
                nuevo_pedido.ID_PEDIDO_MENU = Guid.NewGuid();
                nuevo_pedido.pedido_ID = id_pedido;
                nuevo_pedido.ID_MENU =Guid.Parse(cantidad_menu[i].id_men);
                db_context.AddAsync(nuevo_pedido);



            } }
    
    
    
    
    
    
    
    }

    



}
Boolean comprobacion_existen_reser(string fecha, RestauranteContext dbContext)
{
    bool existen_reser = false;
    foreach (Reserva reserva in dbContext.reserva)
    {
        if (string.Compare(reserva.fecha_de_reseva, fecha) == 0)
        {

            existen_reser = true;
            break;


        }


       
    }
    return existen_reser;



}
app.MapGet("probando_menus_pedido/{id_pedido}", async ([FromServices] RestauranteContext dbcontext, [FromRoute] string id_pedido) => 

{


    return Results.Ok(dbcontext.pedidos_menus.Where(p=>p.pedido_ID==Guid.Parse(id_pedido)));


});


app.MapPost("create_factura/{id_pedido}", async ([FromServices] RestauranteContext dbcontext, [FromRoute]string id_pedido) => 



{
   Guid idpedi= Guid.Parse(id_pedido);
    var pedido_Actual = dbcontext.pedidos.Find(idpedi);
    if (pedido_Actual != null)
    {
        Factura nueva_factura = new Factura();
        nueva_factura.id_factura = Guid.NewGuid();
        nueva_factura.pedidoID = pedido_Actual.id_Pedido;
        nueva_factura.Total = 0;
        nueva_factura.pedido = pedido_Actual;
       
       
        
        dbcontext.AddAsync(nueva_factura);
        dbcontext.SaveChangesAsync();
        return Results.Ok(nueva_factura );

    }
    else return Results.NotFound();


  




});
app.MapGet("ver_factura/{id}", async ([FromServices] RestauranteContext dbcontext, [FromRoute] string id) =>

    {
        //Guid id_pedido = Guid.Parse(id);
        //var factura = dbcontext.facturas.Find(id_pedido);
        //Factura factura_Actual = dbcontext.facturas.Find(id_pedido);


        return Results.Ok(dbcontext.facturas);




    });
app.MapPost("crear_plato", async ([FromServices] RestauranteContext dbcontext, [FromBody] Menu menu) => 

{
    menu.id_menu = Guid.NewGuid();
    dbcontext.AddAsync(menu);
    await dbcontext.SaveChangesAsync();

    return Results.Ok(menu);





});

app.MapGet("ver_categorias", async ([FromServices] RestauranteContext dbcontext) =>
{

    return Results.Ok(dbcontext.categorias);

});

app.Run();
