// Substituir logo do Swagger pela logo personalizada
(function() {
    function replaceLogo() {
        // Procurar pelo logo do Swagger
        const swaggerLogo = document.querySelector('img[alt="Swagger UI"]') || 
                           document.querySelector('.swagger-ui .topbar .download-url-wrapper img') ||
                           document.querySelector('.swagger-ui .topbar img');
        
        if (swaggerLogo && swaggerLogo.src) {
            // Substituir pela logo personalizada
            swaggerLogo.src = '/logo.png';
            swaggerLogo.alt = 'Active Control API';
        }
        
        // Também procurar por outros elementos que possam conter o logo
        const topbar = document.querySelector('.swagger-ui .topbar');
        if (topbar) {
            const existingLogo = topbar.querySelector('img[src*="logo"]');
            if (existingLogo && !existingLogo.src.includes('logo.png')) {
                existingLogo.src = '/logo.png';
                existingLogo.alt = 'Active Control API';
            }
        }
    }
    
    // Executar quando o DOM estiver pronto
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', replaceLogo);
    } else {
        replaceLogo();
    }
    
    // Usar MutationObserver para detectar mudanças dinâmicas
    const observer = new MutationObserver(function(mutations) {
        replaceLogo();
    });
    
    observer.observe(document.body, {
        childList: true,
        subtree: true
    });
    
    // Tentar novamente após um delay
    setTimeout(replaceLogo, 500);
    setTimeout(replaceLogo, 1000);
    setTimeout(replaceLogo, 2000);
})();



