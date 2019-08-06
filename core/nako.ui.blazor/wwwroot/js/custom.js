(function(_0x2f2ex1) {
    'use strict';
    _0x2f2ex1(document)['ready'](function() {
        _0x2f2exe();
        if (_0x2f2ex1('.menu-trigger')['length']) {
            _0x2f2ex1('.menu-trigger')['click'](function() {
                _0x2f2ex1(this)['toggleClass']('active');
                _0x2f2ex1('.header-area .nav')['slideToggle'](200)
            })
        };

        // the menu elements are only added later 
        // when blazor loads its elements
        $(document).bind('DOMNodeInserted', function (event) {
            if (event.target.className == "menu-trigger") {
                if (_0x2f2ex1('.menu-trigger')['length']) {
                    _0x2f2ex1('.menu-trigger')['click'](function () {
                        _0x2f2ex1(this)['toggleClass']('active');
                        _0x2f2ex1('.header-area .nav')['slideToggle'](200)
                    })
                };
            }

            if (event.target.className == "menu-trigger-item") {
                _0x2f2ex1('.menu-trigger-item')['click'](function () {
                    _0x2f2ex1('.menu-trigger')['removeClass']('active');
                    _0x2f2ex1('.header-area .nav')['slideUp'](200)

                })
            }
        });


        _0x2f2ex1('body')['click'](function(_0x2f2ex4) {
            var _0x2f2ex5 = _0x2f2ex4['target'];
            if (_0x2f2ex1(_0x2f2ex5)['parents']('.flag-list')['length'] || _0x2f2ex1(_0x2f2ex5)['hasClass']('flag-list')) {
                return
            };
            if (_0x2f2ex1('.flag-list')['css']('display') === 'block') {
                _0x2f2ex1('.flag-list')['css']('display', 'none');
                return
            };
            if (_0x2f2ex1(_0x2f2ex5)['hasClass']('selected') || _0x2f2ex1(_0x2f2ex5)['parents']('.selected')['length']) {
                _0x2f2ex1('.flag-list')['css']('display', 'block')
            }
        });
        if (_0x2f2ex1('.countdown')['length']) {
            _0x2f2ex1('.countdown')['downCount']({
                date: '09/29/2018 12:00:00',
                offset: +10
            })
        };
        if (_0x2f2ex1('.token .token-input')['length']) {
            _0x2f2ex1('.token .token-input .fa-plus')['click'](function() {
                var _0x2f2ex6 = _0x2f2ex1(this)['parent']()['find']('input')['val']();
                var _0x2f2ex7 = _0x2f2ex1(this)['parent']()['find']('input')['data']('step');
                if (_0x2f2ex6 == '') {
                    _0x2f2ex6 = 0
                };
                var _0x2f2ex8 = parseInt(_0x2f2ex6, 10) + parseInt(_0x2f2ex7, 10);
                _0x2f2ex1(this)['parent']()['find']('input')['val'](_0x2f2ex8)
            });
            _0x2f2ex1('.token .token-input .fa-minus')['click'](function() {
                var _0x2f2ex6 = _0x2f2ex1(this)['parent']()['find']('input')['val']();
                var _0x2f2ex7 = _0x2f2ex1(this)['parent']()['find']('input')['data']('step');
                if (_0x2f2ex6 == '') {
                    _0x2f2ex6 = 0
                };
                var _0x2f2ex8 = parseInt(_0x2f2ex6, 10) - parseInt(_0x2f2ex7, 10);
                if (_0x2f2ex8 <= 0) {
                    _0x2f2ex8 = _0x2f2ex7
                };
                _0x2f2ex1(this)['parent']()['find']('input')['val'](_0x2f2ex8)
            })
        };
        window['sr'] = new scrollReveal();
        _0x2f2ex1('a[href*=\#]:not([href=\#])')['click'](function() {
            if (location['pathname']['replace'](/^\//, '') == this['pathname']['replace'](/^\//, '') && location['hostname'] == this['hostname']) {
                var _0x2f2ex9 = _0x2f2ex1(this['hash']);
                _0x2f2ex9 = _0x2f2ex9['length'] ? _0x2f2ex9 : _0x2f2ex1('[name=' + this['hash']['slice'](1) + ']');
                if (_0x2f2ex9['length']) {
                    var _0x2f2exa = _0x2f2ex1(window)['width']();
                    if (_0x2f2exa < 991) {
                        _0x2f2ex1('.menu-trigger')['removeClass']('active');
                        _0x2f2ex1('.header-area .nav')['slideUp'](200)
                    };
                    _0x2f2ex1('html,body')['animate']({
                        scrollTop: (_0x2f2ex9['offset']()['top']) - 30
                    }, 700);
                    return false
                }
            }
        });
        if (_0x2f2ex1('.token-progress ul')['length']) {
            _0x2f2ex1('.token-progress ul')['find']('.item')['each'](function(_0x2f2exb) {
                _0x2f2ex1('.token-progress ul .item:eq(' + [_0x2f2exb] + ')')['css']('left', _0x2f2ex1('.token-progress ul .item:eq(' + [_0x2f2exb] + ')')['data']('position'))
            });
            var _0x2f2exc = _0x2f2ex1('.token-progress ul .progress-active')['data']('progress');
            _0x2f2ex1('.token-progress ul .progress-active')['css']('width', _0x2f2exc)
        };
        if (_0x2f2ex1('.table-progress')['length']) {
            _0x2f2ex1('.table-latests')['find']('.table-progress')['each'](function(_0x2f2exb) {
                _0x2f2ex1('.table-progress:eq(' + [_0x2f2exb] + ') .progress-line')['css']('width', parseInt(_0x2f2ex1('.table-progress:eq(' + [_0x2f2exb] + ') .progress-line')['data']('value'), 10) + parseInt(70, 10) + '%')
            })
        };
        if (_0x2f2ex1('.roadmap-modern-wrapper')['length']) {
            _0x2f2ex1('.roadmap-modern-wrapper')['owlCarousel']({
                loop: true,
                margin: 30,
                nav: false,
                responsive: {
                    0: {
                        items: 1
                    },
                    600: {
                        items: 2
                    },
                    1000: {
                        items: 3
                    }
                }
            })
        };
        if (_0x2f2ex1('.roadmap-lux-wrapper')['length']) {
            _0x2f2ex1('.roadmap-lux-wrapper')['owlCarousel']({
                loop: true,
                margin: 30,
                nav: false,
                responsive: {
                    0: {
                        items: 1
                    },
                    600: {
                        items: 2
                    },
                    1000: {
                        items: 3
                    }
                }
            })
        }
    });
    _0x2f2ex1(window)['load'](function() {
        _0x2f2ex1('.loading-wrapper')['animate']({
            "\x6F\x70\x61\x63\x69\x74\x79": '0'
        }, 600, function() {
            setTimeout(function() {
                _0x2f2ex1('.loading-wrapper')['css']('visibility', 'hidden')['fadeOut']();
                if (_0x2f2ex1('.parallax')['length']) {
                    _0x2f2ex1('.parallax')['parallax']({
                        imageSrc: 'assets/images/parallax.jpg',
                        zIndex: '1'
                    })
                }
            }, 300)
        })
    });
    _0x2f2ex1(window)['scroll'](function() {
        var _0x2f2exa = _0x2f2ex1(window)['width']();
        if (_0x2f2exa > 991) {
            var _0x2f2exd = _0x2f2ex1(window)['scrollTop']();
            if (_0x2f2exd >= 30) {
                _0x2f2ex1('.header-area')['addClass']('header-sticky');
                _0x2f2ex1('.header-area .dark-logo')['css']('display', 'block');
                _0x2f2ex1('.header-area .light-logo')['css']('display', 'none')
            } else {
                _0x2f2ex1('.header-area')['removeClass']('header-sticky');
                _0x2f2ex1('.header-area .dark-logo')['css']('display', 'none');
                _0x2f2ex1('.header-area .light-logo')['css']('display', 'block')
            }
        }
    });
    _0x2f2ex1(window)['resize'](function() {
        _0x2f2exe()
    });

    function _0x2f2exe() {
        var _0x2f2exa = _0x2f2ex1(window)['width']();
        if (_0x2f2exa > 991) {
            var _0x2f2exf = _0x2f2ex1(window)['height']();
            _0x2f2ex1('.welcome-area')['css']('height', _0x2f2exf - 80)
        } else {
            _0x2f2ex1('.welcome-area')['css']('height', 'auto')
        };
        if (_0x2f2ex1('#home')['length']) {
            particlesJS('home', welcome1Settings)
        }
    }
})(jQuery)
