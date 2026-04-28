document.addEventListener("DOMContentLoaded", function () {
    const toggleSidebarBtn = document.getElementById("toggleSidebar");
    const sidebar = document.getElementById("adminSidebar");
    const overlay = document.getElementById("sidebarOverlay");

    function closeSidebar() {
        sidebar?.classList.remove("show");
        overlay?.classList.remove("show");
    }

    if (toggleSidebarBtn && sidebar && overlay) {
        toggleSidebarBtn.addEventListener("click", function () {
            sidebar.classList.toggle("show");
            overlay.classList.toggle("show");
        });

        overlay.addEventListener("click", closeSidebar);
    }

    document.querySelectorAll(".sidebar-menu a").forEach(link => {
        link.addEventListener("click", function () {
            if (window.innerWidth <= 992) {
                closeSidebar();
            }
        });
    });
});