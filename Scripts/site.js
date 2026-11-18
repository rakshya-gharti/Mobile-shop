(function () {
    var btn = document.getElementById('menuBtn');
    var menu = document.getElementById('sideMenu');
    if (btn && menu) {
        btn.addEventListener('click', function (e) { e.stopPropagation(); menu.classList.toggle('open'); });
        document.addEventListener('click', function (e) { if (!menu.contains(e.target)) menu.classList.remove('open'); });
    }

    // Promotional banner: sliding slides, auto-advance, progress line, prev/next arrows, swipe
    var track = document.querySelector('.banner .track');
    var slides = document.querySelectorAll('.banner .slide');
    var bar = document.querySelector('.banner .progress');
    if (track && slides.length > 1) {
        var i = 0, timer, DELAY = 5000;
        var restartBar = function () {
            if (!bar) return;
            bar.classList.remove('run'); void bar.offsetWidth; bar.classList.add('run');
        };
        var go = function (n) {
            slides[i].classList.remove('active');
            i = (n + slides.length) % slides.length;
            slides[i].classList.add('active');
            track.style.transform = 'translateX(' + (-100 * i) + '%)';
        };
        var start = function () {
            clearInterval(timer);
            timer = setInterval(function () { go(i + 1); restartBar(); }, DELAY);
            restartBar();
        };
        var prev = document.querySelector('.banner .arrow.prev'), next = document.querySelector('.banner .arrow.next');
        if (prev) prev.addEventListener('click', function () { go(i - 1); start(); });
        if (next) next.addEventListener('click', function () { go(i + 1); start(); });
        var x0 = null;
        track.addEventListener('touchstart', function (e) { x0 = e.touches[0].clientX; }, { passive: true });
        track.addEventListener('touchend', function (e) {
            if (x0 === null) return;
            var dx = e.changedTouches[0].clientX - x0; x0 = null;
            if (Math.abs(dx) > 40) { go(dx < 0 ? i + 1 : i - 1); start(); }
        });
        start();
    }

    // Reveal-on-scroll for product cards and section headings
    var items = document.querySelectorAll('.grid .card, .container h1, .container h2, .panel, .cartrow, .stat, table.data tr');
    if ('IntersectionObserver' in window) {
        var io = new IntersectionObserver(function (entries) {
            entries.forEach(function (en) {
                if (en.isIntersecting) { en.target.classList.add('in'); io.unobserve(en.target); }
            });
        }, { threshold: 0.12 });
        items.forEach(function (el, k) {
            el.classList.add('reveal');
            el.style.transitionDelay = ((k % 4) * 80) + 'ms';
            io.observe(el);
        });
    }

    // Soft fade-out when navigating to another page of the site
    var main = document.querySelector('main.container');
    window.addEventListener('pageshow', function () { if (main) main.classList.remove('leaving'); });
    document.addEventListener('click', function (e) {
        var a = e.target.closest ? e.target.closest('a[href]') : null;
        if (!a || !main || e.defaultPrevented || e.metaKey || e.ctrlKey || e.shiftKey || e.button) return;
        var h = a.getAttribute('href');
        if (!h || h.charAt(0) === '#' || /^(javascript|mailto|tel):/i.test(h) || a.target === '_blank' || a.hasAttribute('data-confirm')) return;
        if (a.origin !== location.origin || (a.pathname === location.pathname && a.search === location.search)) return;
        e.preventDefault();
        main.classList.add('leaving');
        setTimeout(function () { location.href = a.href; }, 220);
    });

    // Confirm dialogs for destructive actions
    document.querySelectorAll('[data-confirm]').forEach(function (el) {
        el.addEventListener('click', function (e) {
            if (!confirm(el.getAttribute('data-confirm'))) e.preventDefault();
        });
    });
})();
