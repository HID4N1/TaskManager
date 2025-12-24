// Modern Agile Workspace - Minimal JavaScript

// Sidebar toggle for mobile
document.addEventListener('DOMContentLoaded', function() {
    const sidebar = document.querySelector('.sidebar');
    const toggleButton = document.getElementById('sidebar-toggle');
    
    if (toggleButton) {
        toggleButton.addEventListener('click', function() {
            sidebar.classList.toggle('open');
        });
    }
    
    // Close sidebar when clicking outside on mobile
    if (window.innerWidth <= 768) {
        document.addEventListener('click', function(e) {
            if (sidebar && sidebar.classList.contains('open')) {
                if (!sidebar.contains(e.target) && !toggleButton?.contains(e.target)) {
                    sidebar.classList.remove('open');
                }
            }
        });
    }
    
    // Highlight active sidebar link based on current URL
    const currentPath = window.location.pathname.toLowerCase();
    const sidebarLinks = document.querySelectorAll('.sidebar-nav-link');
    
    sidebarLinks.forEach(link => {
        const href = link.getAttribute('href');
        if (href) {
            const linkPath = href.toLowerCase();
            // Check if current path starts with the link path or matches exactly
            if (currentPath === linkPath || 
                (linkPath !== '/' && currentPath.startsWith(linkPath))) {
                link.classList.add('active');
            }
        }
    });
});

// Form validation enhancement
document.addEventListener('DOMContentLoaded', function() {
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            const requiredFields = form.querySelectorAll('[required]');
            let isValid = true;
            
            requiredFields.forEach(field => {
                if (!field.value.trim()) {
                    isValid = false;
                    field.classList.add('is-invalid');
                } else {
                    field.classList.remove('is-invalid');
                }
            });
            
            if (!isValid) {
                e.preventDefault();
            }
        });
    });
});
