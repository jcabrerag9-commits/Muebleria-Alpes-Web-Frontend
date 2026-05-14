document.addEventListener('DOMContentLoaded', function () {

    // ── Cart count update ──────────────────────────────────────
    function updateCartBadge(count) {
        const badge = document.getElementById('cartCount');
        if (!badge) return;
        badge.textContent = count > 0 ? count : '0';
        badge.style.display = 'flex';
        // Animación de pulso cuando cambia el número
        if (count > 0) {
            badge.classList.remove('badge-pulse');
            void badge.offsetWidth; // forzar reflow
            badge.classList.add('badge-pulse');
        }
    }

    // Cargar el conteo real del carrito desde el servidor
    async function loadCartCount() {
        try {
            const resp = await fetch('/Carrito/Count', { credentials: 'include' });
            if (!resp.ok) return;
            const data = await resp.json();
            updateCartBadge(data.count || 0);
        } catch {
            // Silencioso — el badge queda en 0
        }
    }

    // Ejecutar al cargar la página
    loadCartCount();

    // ── Client-side catalog search ─────────────────────────────
    const searchInput = document.getElementById('catalogSearch');
    if (searchInput) {
        searchInput.addEventListener('keyup', function () {
            const val = this.value.toLowerCase().trim();
            document.querySelectorAll('.producto-card-wrapper').forEach(function (card) {
                const text = card.textContent.toLowerCase();
                card.style.display = val === '' || text.includes(val) ? '' : 'none';
            });

            // Show empty state if all cards hidden
            const emptyMsg = document.getElementById('noResultsMsg');
            if (emptyMsg) {
                const visible = document.querySelectorAll('.producto-card-wrapper:not([style*="display: none"])');
                emptyMsg.style.display = visible.length === 0 ? 'block' : 'none';
            }
        });
    }

    // ── Quantity stepper buttons ───────────────────────────────
    document.querySelectorAll('.qty-increase').forEach(function (btn) {
        btn.addEventListener('click', function () {
            const input = this.closest('.qty-group').querySelector('.qty-input');
            if (input) {
                const max = parseInt(input.getAttribute('max') || '999');
                const current = parseInt(input.value) || 1;
                if (current < max) input.value = current + 1;
            }
        });
    });

    document.querySelectorAll('.qty-decrease').forEach(function (btn) {
        btn.addEventListener('click', function () {
            const input = this.closest('.qty-group').querySelector('.qty-input');
            if (input) {
                const min = parseInt(input.getAttribute('min') || '1');
                const current = parseInt(input.value) || 1;
                if (current > min) input.value = current - 1;
            }
        });
    });

    // ── Auto-dismiss alerts after 5 seconds ───────────────────
    document.querySelectorAll('.alert[data-auto-dismiss]').forEach(function (alert) {
        setTimeout(function () {
            alert.classList.remove('show');
            alert.classList.add('fade');
            setTimeout(function () { alert.remove(); }, 300);
        }, 5000);
    });

});
