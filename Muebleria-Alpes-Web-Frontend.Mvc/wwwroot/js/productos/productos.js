document.addEventListener('DOMContentLoaded', function () {

    // Search / filter
    const buscar = document.getElementById('buscarProducto');
    if (buscar) {
        buscar.addEventListener('keyup', function () {
            const val = this.value.toLowerCase();
            document.querySelectorAll('#tablaProductos tbody tr').forEach(function (row) {
                row.style.display = row.textContent.toLowerCase().includes(val) ? '' : 'none';
            });
        });
    }

    // Detail button with AJAX
    document.querySelectorAll('.btn-detail').forEach(function (btn) {
        btn.addEventListener('click', function () {
            const id = this.dataset.id;

            fetch('/Productos/Detalle/' + id)
                .then(function (r) {
                    if (!r.ok) throw new Error('No encontrado');
                    return r.text();
                })
                .then(function (html) {
                    const container = document.getElementById('detalleProductoContent');
                    if (container) {
                        container.innerHTML = html;
                    }
                    const modalEl = document.getElementById('modalDetalle');
                    if (modalEl) {
                        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
                        modal.show();
                    }
                })
                .catch(function () {
                    alert('No se pudo cargar el detalle del producto.');
                });
        });
    });

});
