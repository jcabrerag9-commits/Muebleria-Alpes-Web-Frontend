document.addEventListener("DOMContentLoaded", function () {
    const inputBuscar = document.getElementById("buscarDepartamento");
    const tabla = document.getElementById("tablaDepartamentos");

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

    document.querySelectorAll(".btn-detail-departamento").forEach(btn => {
        btn.addEventListener("click", async function () {
            const id = this.dataset.id;
            const container = document.getElementById("detalleDepartamentoContainer");

            const response = await fetch(`/Departamentos/Detalle?id=${id}`);
            const html = await response.text();

            container.innerHTML = html;

            const modalElement = document.getElementById("modalDetalleDepartamento");
            const modal = new bootstrap.Modal(modalElement);
            modal.show();
        });
    });
});