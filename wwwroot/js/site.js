$().ready(
    () => {
        detallefactura

    }
);

var detallefactura = () => {
    var leerClientes = new DetalleFactura()
    leerClientes.listaClientes()
}