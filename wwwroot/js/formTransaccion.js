async function llenarListaCategorias(urlCategorias, valorSeleccionado) {
    //hacer peticion para obtener las categorias:
    const respuesta = await fetch(urlCategorias, {
        method: 'POST',
        body: valorSeleccionado,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    const json = await respuesta.json();

    //llenar lista desplegable de categorias:
    const opciones = json.map(categoria => `<option value=${categoria.value}>${categoria.text}</option>`);
    $("#categoriaId").html(opciones);
}