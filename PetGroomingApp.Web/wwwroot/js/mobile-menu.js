// Mobile Menu Toggle Functionality
(function() {
    'use strict';
    
    document.addEventListener('DOMContentLoaded', function() {
        const mobileMenuToggle = document.querySelector('.mobile-menu-toggle');
        const navigation = document.querySelector('#navigation');
        const overlay = document.querySelector('.mobile-menu-overlay');
        
        if (!mobileMenuToggle || !navigation || !overlay) {
            return;
        }
        
        // Toggle menu function
        function toggleMenu() {
            const isActive = navigation.classList.contains('active');
            
            if (isActive) {
                closeMenu();
            } else {
                openMenu();
            }
        }
        
        function openMenu() {
            navigation.classList.add('active');
            overlay.classList.add('active');
            mobileMenuToggle.classList.add('active');
            document.body.style.overflow = 'hidden';
        }
        
        function closeMenu() {
            navigation.classList.remove('active');
            overlay.classList.remove('active');
            mobileMenuToggle.classList.remove('active');
            document.body.style.overflow = '';
            
            // Close all submenus
            const submenus = navigation.querySelectorAll('.submenu');
            submenus.forEach(submenu => {
                submenu.parentElement.classList.remove('active');
            });
        }
        
        // Toggle button click
        mobileMenuToggle.addEventListener('click', function(e) {
            e.stopPropagation();
            toggleMenu();
        });
        
        // Overlay click to close
        overlay.addEventListener('click', function() {
            closeMenu();
        });
        
        // Close on escape key
        document.addEventListener('keydown', function(e) {
            if (e.key === 'Escape' && navigation.classList.contains('active')) {
                closeMenu();
            }
        });
        
        // Handle submenu toggles on mobile
        const menuItemsWithSubmenu = navigation.querySelectorAll('li:has(ul.submenu)');
        menuItemsWithSubmenu.forEach(item => {
            const link = item.querySelector('a');
            if (link) {
                link.addEventListener('click', function(e) {
                    if (window.innerWidth <= 991) {
                        e.preventDefault();
                        item.classList.toggle('active');
                    }
                });
            }
        });
        
        // Close menu when clicking on a link (for navigation)
        const menuLinks = navigation.querySelectorAll('a');
        menuLinks.forEach(link => {
            if (!link.closest('li:has(ul.submenu)')) {
                link.addEventListener('click', function() {
                    if (window.innerWidth <= 991) {
                        closeMenu();
                    }
                });
            }
        });
        
        // Handle window resize
        let resizeTimer;
        window.addEventListener('resize', function() {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(function() {
                if (window.innerWidth > 991) {
                    closeMenu();
                }
            }, 250);
        });
    });
})();

