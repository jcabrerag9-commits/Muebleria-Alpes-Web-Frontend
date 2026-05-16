const CACHE_NAME = "los-alpes-pwa-v1";

const urlsToCache = [
    "/",
    "/manifest.json",
    "/css/site.css",
    "/js/site.js"
];

// INSTALACIÓN
self.addEventListener("install", event => {

    console.log("Service Worker instalado");

    event.waitUntil(

        caches.open(CACHE_NAME)
            .then(cache => {

                console.log("Archivos cacheados");

                return cache.addAll(urlsToCache);
            })
    );

    self.skipWaiting();
});

// ACTIVACIÓN
self.addEventListener("activate", event => {

    console.log("Service Worker activado");

    event.waitUntil(

        caches.keys().then(keys => {

            return Promise.all(

                keys.map(key => {

                    if (key !== CACHE_NAME) {

                        console.log("Cache antigua eliminada:", key);

                        return caches.delete(key);
                    }
                })
            );
        }).then(() => self.clients.claim())
    );
});

// FETCH
self.addEventListener("fetch", event => {

    event.respondWith(

        fetch(event.request)

            .then(response => {

                return response;
            })

            .catch(() => {

                return caches.match(event.request);
            })
    );
});