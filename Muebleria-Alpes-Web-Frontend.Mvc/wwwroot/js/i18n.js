/**
 * i18n.js — Mueblería Los Alpes
 * Simple multilingual engine using data-i18n attributes and JSON translation files.
 *
 * Usage:
 *   i18n.load('es')          — load and apply a language
 *   i18n.t('nav.home')       — get a translated string
 *   i18n.current             — current language code ('es' | 'en')
 *
 * HTML attributes:
 *   data-i18n="nav.home"              — sets element innerHTML
 *   data-i18n-placeholder="key"       — sets input placeholder
 *   data-i18n-title="key"             — sets title attribute
 *   data-i18n-confirm="key"           — stores confirm text; applied via onclick
 */
(function () {
    'use strict';

    var _translations = {};
    var _currentLang = 'es';
    var _cache = {};

    /**
     * Resolve a dot-notation key (e.g. "nav.home") from the translations object.
     */
    function resolve(key) {
        var parts = key.split('.');
        var obj = _translations;
        for (var i = 0; i < parts.length; i++) {
            if (obj == null || typeof obj !== 'object') return null;
            obj = obj[parts[i]];
        }
        return (obj !== undefined && obj !== null) ? String(obj) : null;
    }

    /**
     * Apply all data-i18n* attributes on the page.
     */
    function applyAll() {
        // innerHTML
        document.querySelectorAll('[data-i18n]').forEach(function (el) {
            var key = el.getAttribute('data-i18n');
            var val = resolve(key);
            if (val !== null) el.innerHTML = val;
        });

        // placeholder
        document.querySelectorAll('[data-i18n-placeholder]').forEach(function (el) {
            var key = el.getAttribute('data-i18n-placeholder');
            var val = resolve(key);
            if (val !== null) el.setAttribute('placeholder', val);
        });

        // title
        document.querySelectorAll('[data-i18n-title]').forEach(function (el) {
            var key = el.getAttribute('data-i18n-title');
            var val = resolve(key);
            if (val !== null) el.setAttribute('title', val);
        });

        // confirm dialogs — replace onclick confirm text
        document.querySelectorAll('[data-i18n-confirm]').forEach(function (el) {
            var key = el.getAttribute('data-i18n-confirm');
            var val = resolve(key);
            if (val !== null) {
                el.setAttribute('onclick', "return confirm('" + val.replace(/'/g, "\\'") + "')");
            }
        });

        // Update html lang attribute
        document.documentElement.setAttribute('lang', _currentLang);

        // Highlight active lang button
        document.querySelectorAll('[data-lang-btn]').forEach(function (btn) {
            var lang = btn.getAttribute('data-lang-btn');
            if (lang === _currentLang) {
                btn.classList.add('lang-active');
            } else {
                btn.classList.remove('lang-active');
            }
        });
    }

    /**
     * Load a language from /lang/{lang}.json, apply translations, and persist choice.
     */
    function load(lang) {
        if (!lang) lang = 'es';
        lang = lang.toLowerCase();

        // Use cached translations if available
        if (_cache[lang]) {
            _translations = _cache[lang];
            _currentLang = lang;
            localStorage.setItem('alpes_lang', lang);
            applyAll();
            return Promise.resolve();
        }

        return fetch('/lang/' + lang + '.json?v=' + Date.now())
            .then(function (r) {
                if (!r.ok) throw new Error('Language file not found: ' + lang);
                return r.json();
            })
            .then(function (data) {
                _cache[lang] = data;
                _translations = data;
                _currentLang = lang;
                localStorage.setItem('alpes_lang', lang);
                applyAll();
            })
            .catch(function (err) {
                console.warn('[i18n] Could not load language "' + lang + '":', err);
                // Fallback to Spanish if something fails
                if (lang !== 'es') load('es');
            });
    }

    /**
     * Translate a key, optionally replacing {{count}} with a number.
     */
    function t(key, count) {
        var val = resolve(key);
        if (val === null) return key; // return key as fallback
        if (count !== undefined) val = val.replace('{{count}}', count);
        return val;
    }

    /**
     * Initialize: read persisted language preference and apply.
     */
    function init() {
        var saved = localStorage.getItem('alpes_lang') || 'es';
        load(saved);
    }

    // Public API
    window.i18n = {
        load: load,
        t: t,
        apply: applyAll,
        get current() { return _currentLang; }
    };

    // Auto-initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
