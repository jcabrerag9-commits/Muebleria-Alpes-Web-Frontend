document.addEventListener("DOMContentLoaded", function () {
    const inputBuscar = document.getElementById("buscarEmpleado");
    const tabla = document.getElementById("tablaEmpleados");

    if (inputBuscar && tabla) {
        inputBuscar.addEventListener("keyup", function () {
            const filtro = inputBuscar.value.toLowerCase();
            const filas = tabla.querySelectorAll("tbody tr");

            filas.forEach(fila => {
                const texto = fila.innerText.toLowerCase();
                fila.style.display = texto.includes(filtro) ? "" : "none";
            });
        });
    }

    document.querySelectorAll(".btn-detail").forEach(btn => {
        btn.addEventListener("click", async function () {
            const id = this.dataset.id;
            const container = document.getElementById("detalleEmpleadoContainer");

            const response = await fetch(`/Empleados/Detalle?id=${id}`);
            const html = await response.text();

            container.innerHTML = html;

            const modalElement = document.getElementById("modalDetalleEmpleado");
            const modal = new bootstrap.Modal(modalElement);
            modal.show();
        });
    });
});