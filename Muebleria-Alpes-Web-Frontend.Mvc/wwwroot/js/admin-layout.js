document.addEventListener("DOMContentLoaded", function () {
    const toggleSidebarBtn = document.getElementById("toggleSidebar");
    const sidebar = document.getElementById("adminSidebar");

    if (toggleSidebarBtn && sidebar) {
        toggleSidebarBtn.addEventListener("click", function () {
            sidebar.classList.toggle("show");
        });
    }
});