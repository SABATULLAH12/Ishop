﻿/*! jQuery UI - v1.10.3 - 2013-08-11
 * http://jqueryui.com
 * Includes: jquery.ui.core.js, jquery.ui.widget.js, jquery.ui.mouse.js, jquery.ui.position.js, jquery.ui.draggable.js, jquery.ui.droppable.js, jquery.ui.resizable.js, jquery.ui.selectable.js, jquery.ui.sortable.js, jquery.ui.accordion.js, jquery.ui.slider.js, jquery.ui.tabs.js, jquery.ui.effect.js, jquery.ui.effect-blind.js, jquery.ui.effect-bounce.js, jquery.ui.effect-clip.js, jquery.ui.effect-drop.js, jquery.ui.effect-explode.js, jquery.ui.effect-fade.js, jquery.ui.effect-fold.js, jquery.ui.effect-highlight.js, jquery.ui.effect-pulsate.js, jquery.ui.effect-scale.js, jquery.ui.effect-shake.js, jquery.ui.effect-slide.js, jquery.ui.effect-transfer.js
 * Copyright 2013 jQuery Foundation and other contributors Licensed MIT */
(function (e, t) {
  
    function i(t, i) {
        var a, n, r, o = t.nodeName.toLowerCase();
        return "area" === o ? (a = t.parentNode,
			n = a.name, t.href && n && "map" ===
			a.nodeName.toLowerCase() ? (r = e(
					"img[usemap=#" + n + "]")[0], !!
				r && s(r)) : !1) : (
			/input|select|textarea|button|object/
			.test(o) ? !t.disabled : "a" === o ?
			t.href || i : i) && s(t)
    }

    function s(t) {
        return e.expr.filters.visible(t) && !
			e(t).parents().addBack().filter(
				function () {
				    return "hidden" === e.css(this,
						"visibility")
				}).length
    }
    var a = 0,
		n = /^ui-id-\d+$/;
    e.ui = e.ui || {}, e.extend(e.ui, {
        version: "1.10.3",
        keyCode: {
            BACKSPACE: 8,
            COMMA: 188,
            DELETE: 46,
            DOWN: 40,
            END: 35,
            ENTER: 13,
            ESCAPE: 27,
            HOME: 36,
            LEFT: 37,
            NUMPAD_ADD: 107,
            NUMPAD_DECIMAL: 110,
            NUMPAD_DIVIDE: 111,
            NUMPAD_ENTER: 108,
            NUMPAD_MULTIPLY: 106,
            NUMPAD_SUBTRACT: 109,
            PAGE_DOWN: 34,
            PAGE_UP: 33,
            PERIOD: 190,
            RIGHT: 39,
            SPACE: 32,
            TAB: 9,
            UP: 38
        }
    }), e.fn.extend({
        focus: function (t) {
            return function (i, s) {
                return "number" == typeof i ?
					this.each(function () {
					    var t = this;
					    setTimeout(function () {
					        e(t).focus(), s && s.call(t)
					    }, i)
					}) : t.apply(this, arguments)
            }
        }(e.fn.focus),
        scrollParent: function () {
            var t;
            return t = e.ui.ie &&
				/(static|relative)/.test(this.css(
					"position")) || /absolute/.test(
					this.css("position")) ? this.parents()
				.filter(function () {

				    

				    return /(relative|absolute|fixed)/
						.test(e.css(this, "position")) &&
						/(auto|scroll)/.test(e.css(
							this, "overflow") + e.css(
							this, "overflow-y") + e.css(
							this, "overflow-x"))
				}).eq(0) : this.parents().filter(
					function () {
					    return /(auto|scroll)/.test(e.css(
							this, "overflow") + e.css(
							this, "overflow-y") + e.css(
							this, "overflow-x"))
					}).eq(0), /fixed/.test(this.css(
					"position")) || !t.length ? e(
					document) : t
        },
        zIndex: function (i) {
            if (i !== t) return this.css(
				"zIndex", i);
            if (this.length)
                for (var s, a, n = e(this[0]) ; n.length &&
					n[0] !== document;) {
                    if (s = n.css("position"), (
						"absolute" === s || "relative" ===
						s || "fixed" === s) && (a =
						parseInt(n.css("zIndex"), 10), !
						isNaN(a) && 0 !== a)) return a;
                    n = n.parent()
                }
            return 0
        },
        uniqueId: function () {
            return this.each(function () {
                this.id || (this.id = "ui-id-" + ++
					a)
            })
        },
        removeUniqueId: function () {
            return this.each(function () {
                n.test(this.id) && e(this).removeAttr(
					"id")
            })
        }
    }), e.extend(e.expr[":"], {
        data: e.expr.createPseudo ? e.expr.createPseudo(
			function (t) {
			    return function (i) {
			        return !!e.data(i, t)
			    }
			}) : function (t, i, s) {
			    return !!e.data(t, s[3])
			},
        focusable: function (t) {
            return i(t, !isNaN(e.attr(t,
				"tabindex")))
        },
        tabbable: function (t) {
            var s = e.attr(t, "tabindex"),
				a = isNaN(s);
            return (a || s >= 0) && i(t, !a)
        }
    }), e("<a>").outerWidth(1).jquery ||
		e.each(["Width", "Height"], function (
			i, s) {
		    function a(t, i, s, a) {
		        return e.each(n, function () {
		            i -= parseFloat(e.css(t,
						"padding" + this)) || 0, s &&
						(i -= parseFloat(e.css(t,
							"border" + this + "Width")) ||
						0), a && (i -= parseFloat(e.css(
						t, "margin" + this)) || 0)
		        }), i
		    }
		    var n = "Width" === s ? ["Left",
				"Right"
		    ] : ["Top", "Bottom"],
				r = s.toLowerCase(),
				o = {
				    innerWidth: e.fn.innerWidth,
				    innerHeight: e.fn.innerHeight,
				    outerWidth: e.fn.outerWidth,
				    outerHeight: e.fn.outerHeight
				};
		    e.fn["inner" + s] = function (i) {
		        return i === t ? o["inner" + s].call(
					this) : this.each(function () {
					    e(this).css(r, a(this, i) +
                            "px")
					})
		    }, e.fn["outer" + s] = function (t,
				i) {
		        return "number" != typeof t ? o[
					"outer" + s].call(this, t) :
					this.each(function () {
					    e(this).css(r, a(this, t, !0,
							i) + "px")
					})
		    }
		}), e.fn.addBack || (e.fn.addBack =
			function (e) {
			    return this.add(null == e ? this.prevObject :
					this.prevObject.filter(e))
			}), e("<a>").data("a-b", "a").removeData(
			"a-b").data("a-b") && (e.fn.removeData =
			function (t) {
			    return function (i) {
			        return arguments.length ? t.call(
						this, e.camelCase(i)) : t.call(
						this)
			    }
			}(e.fn.removeData)), e.ui.ie = !!
		/msie [\w.]+/.exec(navigator.userAgent
			.toLowerCase()), e.support.selectstart =
		"onselectstart" in document.createElement(
			"div"), e.fn.extend({
			    disableSelection: function () {
			        return this.bind((e.support.selectstart ?
                            "selectstart" : "mousedown") +
                        ".ui-disableSelection", function (
                            e) {
                            e.preventDefault()
                        })
			    },
			    enableSelection: function () {
			        return this.unbind(
                        ".ui-disableSelection")
			    }
			}), e.extend(e.ui, {
			    plugin: {
			        add: function (t, i, s) {
			            var a, n = e.ui[t].prototype;
			            for (a in s) n.plugins[a] = n.plugins[
                            a] || [], n.plugins[a].push([i,
                            s[a]
                            ])
			        },
			        call: function (e, t, i) {
			            var s, a = e.plugins[t];
			            if (a && e.element[0].parentNode &&
                            11 !== e.element[0].parentNode.nodeType
                        )
			                for (s = 0; a.length > s; s++) e
                                .options[a[s][0]] && a[s][1].apply(
                                    e.element, i)
			        }
			    },
			    hasScroll: function (t, i) {
			        if ("hidden" === e(t).css(
                        "overflow")) return !1;
			        var s = i && "left" === i ?
                        "scrollLeft" : "scrollTop",
                        a = !1;
			        return t[s] > 0 ? !0 : (t[s] = 1,
                        a = t[s] > 0, t[s] = 0, a)
			    }
			})
})(jQuery);
(function (e, t) {
    var i = 0,
		s = Array.prototype.slice,
		n = e.cleanData;
    e.cleanData = function (t) {
        for (var i, s = 0; null != (i = t[s]) ; s++)
            try {
                e(i).triggerHandler("remove")
            } catch (a) { }
        n(t)
    }, e.widget = function (i, s, n) {
        var a, r, o, h, l = {}, u = i.split(
				".")[0];
        i = i.split(".")[1], a = u + "-" + i,
			n || (n = s, s = e.Widget), e.expr[
				":"][a.toLowerCase()] = function (
				t) {
				    return !!e.data(t, a)
				}, e[u] = e[u] || {}, r = e[u][i], o =
			e[u][i] = function (e, i) {
			    return this._createWidget ? (
					arguments.length && this._createWidget(
						e, i), t) : new o(e, i)
			}, e.extend(o, r, {
			    version: n.version,
			    _proto: e.extend({}, n),
			    _childConstructors: []
			}), h = new s, h.options = e.widget.extend({},
			h.options), e.each(n, function (i,
			n) {
			    return e.isFunction(n) ? (l[i] =
                    function () {
                        var e = function () {
                            return s.prototype[i].apply(
                                this, arguments)
                        }, t = function (e) {
                            return s.prototype[i].apply(
								this, e)
                        };
                        return function () {
                            var i, s = this._super,
                                a = this._superApply;
                            return this._super = e, this._superApply =
                                t, i = n.apply(this, arguments),
                                this._super = s, this._superApply =
                                a, i
                        }
                    }(), t) : (l[i] = n, t)
			}), o.prototype = e.widget.extend(h, {
			    widgetEventPrefix: r ? h.widgetEventPrefix : i
			}, l, {
			    constructor: o,
			    namespace: u,
			    widgetName: i,
			    widgetFullName: a
			}), r ? (e.each(r._childConstructors,
			function (t, i) {
			    var s = i.prototype;
			    e.widget(s.namespace + "." + s.widgetName,
					o, i._proto)
			}), delete r._childConstructors) :
			s._childConstructors.push(o), e.widget
			.bridge(i, o)
    }, e.widget.extend = function (i) {
        for (var n, a, r = s.call(arguments,
				1), o = 0, h = r.length; h > o; o++)
            for (n in r[o]) a = r[o][n], r[o].hasOwnProperty(
				n) && a !== t && (i[n] = e.isPlainObject(
					a) ? e.isPlainObject(i[n]) ? e.widget
				.extend({}, i[n], a) : e.widget.extend({},
					a) : a);
        return i
    }, e.widget.bridge = function (i, n) {
        var a = n.prototype.widgetFullName ||
			i;
        e.fn[i] = function (r) {
            var o = "string" == typeof r,
				h = s.call(arguments, 1),
				l = this;
            return r = !o && h.length ? e.widget
				.extend.apply(null, [r].concat(h)) :
				r, o ? this.each(function () {
				    var s, n = e.data(this, a);
				    return n ? e.isFunction(n[r]) &&
						"_" !== r.charAt(0) ? (s = n[r]
							.apply(n, h), s !== n && s !==
							t ? (l = s && s.jquery ? l.pushStack(
								s.get()) : s, !1) : t) : e.error(
							"no such method '" + r +
							"' for " + i +
							" widget instance") : e.error(
							"cannot call methods on " + i +
							" prior to initialization; " +
							"attempted to call method '" +
							r + "'")
				}) : this.each(function () {
				    var t = e.data(this, a);
				    t ? t.option(r || {})._init() :
						e.data(this, a, new n(r, this))
				}), l
        }
    }, e.Widget = function () { }, e.Widget
		._childConstructors = [], e.Widget.prototype = {
		    widgetName: "widget",
		    widgetEventPrefix: "",
		    defaultElement: "<div>",
		    options: {
		        disabled: !1,
		        create: null
		    },
		    _createWidget: function (t, s) {
		        s = e(s || this.defaultElement ||
					this)[0], this.element = e(s),
					this.uuid = i++, this.eventNamespace =
					"." + this.widgetName + this.uuid,
					this.options = e.widget.extend({},
						this.options, this._getCreateOptions(),
						t), this.bindings = e(), this.hoverable =
					e(), this.focusable = e(), s !==
					this && (e.data(s, this.widgetFullName,
						this), this._on(!0, this.element, {
						    remove: function (e) {
						        e.target === s && this.destroy()
						    }
						}), this.document = e(s.style ?
						s.ownerDocument : s.document ||
						s), this.window = e(this.document[
						0].defaultView || this.document[
						0].parentWindow)), this._create(),
					this._trigger("create", null,
						this._getCreateEventData()),
					this._init()
		    },
		    _getCreateOptions: e.noop,
		    _getCreateEventData: e.noop,
		    _create: e.noop,
		    _init: e.noop,
		    destroy: function () {
		        this._destroy(), this.element.unbind(
					this.eventNamespace).removeData(
					this.widgetName).removeData(this.widgetFullName)
					.removeData(e.camelCase(this.widgetFullName)),
					this.widget().unbind(this.eventNamespace)
					.removeAttr("aria-disabled").removeClass(
						this.widgetFullName +
						"-disabled " +
						"ui-state-disabled"), this.bindings
					.unbind(this.eventNamespace),
					this.hoverable.removeClass(
						"ui-state-hover"), this.focusable
					.removeClass("ui-state-focus")
		    },
		    _destroy: e.noop,
		    widget: function () {
		        return this.element
		    },
		    option: function (i, s) {
		        var n, a, r, o = i;
		        if (0 === arguments.length) return e
					.widget.extend({}, this.options);
		        if ("string" == typeof i)
		            if (o = {}, n = i.split("."), i =
						n.shift(), n.length) {
		                for (a = o[i] = e.widget.extend({},
								this.options[i]), r = 0; n.length -
							1 > r; r++) a[n[r]] = a[n[r]] || {},
							a = a[n[r]];
		                if (i = n.pop(), s === t) return a[
							i] === t ? null : a[i];
		                a[i] = s
		            } else {
		                if (s === t) return this.options[
							i] === t ? null : this.options[
							i];
		                o[i] = s
		            }
		        return this._setOptions(o), this
		    },
		    _setOptions: function (e) {
		        var t;
		        for (t in e) this._setOption(t, e[
					t]);
		        return this
		    },
		    _setOption: function (e, t) {
		        return this.options[e] = t,
					"disabled" === e && (this.widget()
						.toggleClass(this.widgetFullName +
							"-disabled ui-state-disabled", !!
							t).attr("aria-disabled", t),
						this.hoverable.removeClass(
							"ui-state-hover"), this.focusable
						.removeClass("ui-state-focus")),
					this
		    },
		    enable: function () {
		        return this._setOption("disabled", !
					1)
		    },
		    disable: function () {
		        return this._setOption("disabled", !
					0)
		    },
		    _on: function (i, s, n) {
		        var a, r = this;
		        "boolean" != typeof i && (n = s, s =
					i, i = !1), n ? (s = a = e(s),
					this.bindings = this.bindings.add(
						s)) : (n = s, s = this.element,
					a = this.widget()), e.each(n,
					function (n, o) {
					    function h() {
					        return i || r.options.disabled !== !
								0 && !e(this).hasClass(
									"ui-state-disabled") ? (
									"string" == typeof o ? r[o] :
									o).apply(r, arguments) : t
					    }
					    "string" != typeof o && (h.guid =
							o.guid = o.guid || h.guid || e.guid++
						);
					    var l = n.match(/^(\w+)\s*(.*)$/),
							u = l[1] + r.eventNamespace,
							c = l[2];
					    c ? a.delegate(c, u, h) : s.bind(
							u, h)
					})
		    },
		    _off: function (e, t) {
		        t = (t || "").split(" ").join(this
					.eventNamespace + " ") + this.eventNamespace,
					e.unbind(t).undelegate(t)
		    },
		    _delay: function (e, t) {
		        function i() {
		            return ("string" == typeof e ? s[
						e] : e).apply(s, arguments)
		        }
		        var s = this;
		        return setTimeout(i, t || 0)
		    },
		    _hoverable: function (t) {
		        this.hoverable = this.hoverable.add(
					t), this._on(t, {
					    mouseenter: function (t) {
					        e(t.currentTarget).addClass(
                                "ui-state-hover")
					    },
					    mouseleave: function (t) {
					        e(t.currentTarget).removeClass(
                                "ui-state-hover")
					    }
					})
		    },
		    _focusable: function (t) {
		        this.focusable = this.focusable.add(
					t), this._on(t, {
					    focusin: function (t) {
					        e(t.currentTarget).addClass(
                                "ui-state-focus")
					    },
					    focusout: function (t) {
					        e(t.currentTarget).removeClass(
                                "ui-state-focus")
					    }
					})
		    },
		    _trigger: function (t, i, s) {
		        var n, a, r = this.options[t];
		        if (s = s || {}, i = e.Event(i), i
					.type = (t === this.widgetEventPrefix ?
		            t : this.widgetEventPrefix + t).toLowerCase(),
					i.target = this.element[0], a = i
					.originalEvent)
		            for (n in a) n in i || (i[n] = a[
						n]);
		        return this.element.trigger(i, s), !
					(e.isFunction(r) && r.apply(this.element[
					0], [i].concat(s)) === !1 || i.isDefaultPrevented())
		    }
		}, e.each({
		    show: "fadeIn",
		    hide: "fadeOut"
		}, function (t, i) {
		    e.Widget.prototype["_" + t] =
                function (s, n, a) {
                    "string" == typeof n && (n = {
                        effect: n
                    });
                    var r, o = n ? n === !0 ||
                            "number" == typeof n ? i : n.effect ||
                        i : t;
                    n = n || {}, "number" == typeof n &&
                        (n = {
                            duration: n
                        }), r = !e.isEmptyObject(n), n.complete =
                        a, n.delay && s.delay(n.delay),
                        r && e.effects && e.effects.effect[
                            o] ? s[t](n) : o !== t && s[o] ?
                        s[o](n.duration, n.easing, a) :
                        s.queue(function (i) {
                            e(this)[t](), a && a.call(s[0]),
                                i()
                        })
                }
		})
})(jQuery);
(function (e) {
    var t = !1;
    e(document).mouseup(function () {
        t = !1
    }), e.widget("ui.mouse", {
        version: "1.10.3",
        options: {
            cancel: "input,textarea,button,select,option",
            distance: 1,
            delay: 0
        },
        _mouseInit: function () {
            var t = this;
            this.element.bind("mousedown." +
				this.widgetName, function (e) {
				    return t._mouseDown(e)
				}).bind("click." + this.widgetName,
				function (i) {
				    return !0 === e.data(i.target, t
						.widgetName +
						".preventClickEvent") ? (e.removeData(
							i.target, t.widgetName +
							".preventClickEvent"), i.stopImmediatePropagation(), !
						1) : undefined
				}), this.started = !1
        },
        _mouseDestroy: function () {
            this.element.unbind("." + this.widgetName),
				this._mouseMoveDelegate && e(
					document).unbind("mousemove." +
					this.widgetName, this._mouseMoveDelegate
			).unbind("mouseup." + this.widgetName,
				this._mouseUpDelegate)
        },
        _mouseDown: function (i) {
            if (!t) {
                this._mouseStarted && this._mouseUp(
					i), this._mouseDownEvent = i;
                var s = this,
					n = 1 === i.which,
					a = "string" == typeof this.options
						.cancel && i.target.nodeName ?
						e(i.target).closest(this.options
							.cancel).length : !1;
                return n && !a && this._mouseCapture(
					i) ? (this.mouseDelayMet = !this
					.options.delay, this.mouseDelayMet ||
					(this._mouseDelayTimer =
						setTimeout(function () {
						    s.mouseDelayMet = !0
						}, this.options.delay)), this._mouseDistanceMet(
						i) && this._mouseDelayMet(i) &&
					(this._mouseStarted = this._mouseStart(
						i) !== !1, !this._mouseStarted) ?
					(i.preventDefault(), !0) : (!0 ===
						e.data(i.target, this.widgetName +
							".preventClickEvent") && e.removeData(
							i.target, this.widgetName +
							".preventClickEvent"), this._mouseMoveDelegate =
						function (e) {
						    return s._mouseMove(e)
						}, this._mouseUpDelegate =
						function (e) {
						    return s._mouseUp(e)
						}, e(document).bind(
							"mousemove." + this.widgetName,
							this._mouseMoveDelegate).bind(
							"mouseup." + this.widgetName,
							this._mouseUpDelegate), i.preventDefault(),
						t = !0, !0)) : !0
            }
        },
        _mouseMove: function (t) {
            return e.ui.ie && (!document.documentMode ||
				9 > document.documentMode) && !t.button ?
				this._mouseUp(t) : this._mouseStarted ?
				(this._mouseDrag(t), t.preventDefault()) :
				(this._mouseDistanceMet(t) &&
				this._mouseDelayMet(t) && (this._mouseStarted =
					this._mouseStart(this._mouseDownEvent,
						t) !== !1, this._mouseStarted ?
					this._mouseDrag(t) : this._mouseUp(
						t)), !this._mouseStarted)
        },
        _mouseUp: function (t) {
            return e(document).unbind(
				"mousemove." + this.widgetName,
				this._mouseMoveDelegate).unbind(
				"mouseup." + this.widgetName,
				this._mouseUpDelegate), this._mouseStarted &&
				(this._mouseStarted = !1, t.target ===
				this._mouseDownEvent.target && e.data(
					t.target, this.widgetName +
					".preventClickEvent", !0), this._mouseStop(
					t)), !1
        },
        _mouseDistanceMet: function (e) {
            return Math.max(Math.abs(this._mouseDownEvent
				.pageX - e.pageX), Math.abs(this
				._mouseDownEvent.pageY - e.pageY
			)) >= this.options.distance
        },
        _mouseDelayMet: function () {
            return this.mouseDelayMet
        },
        _mouseStart: function () { },
        _mouseDrag: function () { },
        _mouseStop: function () { },
        _mouseCapture: function () {
            return !0
        }
    })
})(jQuery);
(function (t, e) {
    function i(t, e, i) {
        return [parseFloat(t[0]) * (p.test(t[
				0]) ? e / 100 : 1), parseFloat(t[1]) *
			(p.test(t[1]) ? i / 100 : 1)]
    }

    function s(e, i) {
        return parseInt(t.css(e, i), 10) ||
			0
    }

    function n(e) {
        var i = e[0];
        return 9 === i.nodeType ? {
            width: e.width(),
            height: e.height(),
            offset: {
                top: 0,
                left: 0
            }
        } : t.isWindow(i) ? {
            width: e.width(),
            height: e.height(),
            offset: {
                top: e.scrollTop(),
                left: e.scrollLeft()
            }
        } : i.preventDefault ? {
            width: 0,
            height: 0,
            offset: {
                top: i.pageY,
                left: i.pageX
            }
        } : {
            width: e.outerWidth(),
            height: e.outerHeight(),
            offset: e.offset()
        }
    }
    t.ui = t.ui || {};
    var a, o = Math.max,
		r = Math.abs,
		h = Math.round,
		l = /left|center|right/,
		c = /top|center|bottom/,
		u = /[\+\-]\d+(\.[\d]+)?%?/,
		d = /^\w+/,
		p = /%$/,
		f = t.fn.position;
    t.position = {
        scrollbarWidth: function () {
            if (a !== e) return a;
            var i, s, n = t(
					"<div style='display:block;width:50px;height:50px;overflow:hidden;'><div style='height:100px;width:auto;'></div></div>"
				),
				o = n.children()[0];
            return t("body").append(n), i = o.offsetWidth,
				n.css("overflow", "scroll"), s = o
				.offsetWidth, i === s && (s = n[0]
					.clientWidth), n.remove(), a = i -
				s
        },
        getScrollInfo: function (e) {
            var i = e.isWindow ? "" : e.element
				.css("overflow-x"),
				s = e.isWindow ? "" : e.element.css(
					"overflow-y"),
				n = "scroll" === i || "auto" === i &&
					e.width < e.element[0].scrollWidth,
				a = "scroll" === s || "auto" === s &&
					e.height < e.element[0].scrollHeight;
            return {
                width: a ? t.position.scrollbarWidth() : 0,
                height: n ? t.position.scrollbarWidth() : 0
            }
        },
        getWithinInfo: function (e) {
            var i = t(e || window),
				s = t.isWindow(i[0]);
            return {
                element: i,
                isWindow: s,
                offset: i.offset() || {
                    left: 0,
                    top: 0
                },
                scrollLeft: i.scrollLeft(),
                scrollTop: i.scrollTop(),
                width: s ? i.width() : i.outerWidth(),
                height: s ? i.height() : i.outerHeight()
            }
        }
    }, t.fn.position = function (e) {
        if (!e || !e.of) return f.apply(this,
			arguments);
        e = t.extend({}, e);
        var a, p, m, g, v, b, _ = t(e.of),
			y = t.position.getWithinInfo(e.within),
			w = t.position.getScrollInfo(y),
			x = (e.collision || "flip").split(
				" "),
			k = {};
        return b = n(_), _[0].preventDefault &&
			(e.at = "left top"), p = b.width, m =
			b.height, g = b.offset, v = t.extend({},
				g), t.each(["my", "at"], function () {
				    var t, i, s = (e[this] || "").split(
                            " ");
				    1 === s.length && (s = l.test(s[0]) ?
                        s.concat(["center"]) : c.test(s[
                            0]) ? ["center"].concat(s) : [
                            "center", "center"
                            ]), s[0] = l.test(s[0]) ? s[0] :
                        "center", s[1] = c.test(s[1]) ?
                        s[1] : "center", t = u.exec(s[0]),
                        i = u.exec(s[1]), k[this] = [t ?
                            t[0] : 0, i ? i[0] : 0
                        ], e[this] = [d.exec(s[0])[0], d.exec(
                        s[1])[0]]
				}), 1 === x.length && (x[1] = x[0]),
			"right" === e.at[0] ? v.left += p :
			"center" === e.at[0] && (v.left +=
				p / 2), "bottom" === e.at[1] ? v.top +=
            m : "center" === e.at[1] && (v.top +=
				m / 2), a = i(k.at, p, m), v.left +=
			a[0], v.top += a[1], this.each(
				function () {
				    var n, l, c = t(this),
						u = c.outerWidth(),
						d = c.outerHeight(),
						f = s(this, "marginLeft"),
						b = s(this, "marginTop"),
						D = u + f + s(this,
							"marginRight") + w.width,
						T = d + b + s(this,
							"marginBottom") + w.height,
						C = t.extend({}, v),
						M = i(k.my, c.outerWidth(), c.outerHeight());
				    "right" === e.my[0] ? C.left -= u :
						"center" === e.my[0] && (C.left -=
							u / 2), "bottom" === e.my[1] ?
						C.top -= d : "center" === e.my[1] &&
						(C.top -= d / 2), C.left += M[0],
						C.top += M[1], t.support.offsetFractions ||
						(C.left = h(C.left), C.top = h(C
						.top)), n = {
						    marginLeft: f,
						    marginTop: b
						}, t.each(["left", "top"],
						function (i, s) {
						    t.ui.position[x[i]] && t.ui.position[
								x[i]][s](C, {
								    targetWidth: p,
								    targetHeight: m,
								    elemWidth: u,
								    elemHeight: d,
								    collisionPosition: n,
								    collisionWidth: D,
								    collisionHeight: T,
								    offset: [a[0] + M[0], a[1] +
                                        M[1]
								    ],
								    my: e.my,
								    at: e.at,
								    within: y,
								    elem: c
								})
						}), e.using && (l = function (t) {
						    var i = g.left - C.left,
                                s = i + p - u,
                                n = g.top - C.top,
                                a = n + m - d,
                                h = {
                                    target: {
                                        element: _,
                                        left: g.left,
                                        top: g.top,
                                        width: p,
                                        height: m
                                    },
                                    element: {
                                        element: c,
                                        left: C.left,
                                        top: C.top,
                                        width: u,
                                        height: d
                                    },
                                    horizontal: 0 > s ? "left" : i >
                                        0 ? "right" : "center",
                                    vertical: 0 > a ? "top" : n >
                                        0 ? "bottom" : "middle"
                                };
						    u > p && p > r(i + s) && (h.horizontal =
                                "center"), d > m && m > r(n +
                                a) && (h.vertical = "middle"),
                                h.important = o(r(i), r(s)) >
                                o(r(n), r(a)) ? "horizontal" :
                                "vertical", e.using.call(this,
                                    t, h)
						}), c.offset(t.extend(C, {
						    using: l
						}))
				})
    }, t.ui.position = {
        fit: {
            left: function (t, e) {
                var i, s = e.within,
					n = s.isWindow ? s.scrollLeft : s
						.offset.left,
					a = s.width,
					r = t.left - e.collisionPosition.marginLeft,
					h = n - r,
					l = r + e.collisionWidth - a - n;
                e.collisionWidth > a ? h > 0 && 0 >=
					l ? (i = t.left + h + e.collisionWidth -
						a - n, t.left += h - i) : t.left =
					l > 0 && 0 >= h ? n : h > l ? n +
					a - e.collisionWidth : n : h > 0 ?
					t.left += h : l > 0 ? t.left -= l :
					t.left = o(t.left - r, t.left)
            },
            top: function (t, e) {
                var i, s = e.within,
					n = s.isWindow ? s.scrollTop : s.offset
						.top,
					a = e.within.height,
					r = t.top - e.collisionPosition.marginTop,
					h = n - r,
					l = r + e.collisionHeight - a - n;
                e.collisionHeight > a ? h > 0 && 0 >=
					l ? (i = t.top + h + e.collisionHeight -
						a - n, t.top += h - i) : t.top =
					l > 0 && 0 >= h ? n : h > l ? n +
					a - e.collisionHeight : n : h > 0 ?
					t.top += h : l > 0 ? t.top -= l :
					t.top = o(t.top - r, t.top)
            }
        },
        flip: {
            left: function (t, e) {
                var i, s, n = e.within,
					a = n.offset.left + n.scrollLeft,
					o = n.width,
					h = n.isWindow ? n.scrollLeft : n
						.offset.left,
					l = t.left - e.collisionPosition.marginLeft,
					c = l - h,
					u = l + e.collisionWidth - o - h,
					d = "left" === e.my[0] ? -e.elemWidth :
						"right" === e.my[0] ? e.elemWidth :
						0,
					p = "left" === e.at[0] ? e.targetWidth :
						"right" === e.at[0] ? -e.targetWidth :
						0,
					f = -2 * e.offset[0];
                0 > c ? (i = t.left + d + p + f +
					e.collisionWidth - o - a, (0 > i ||
						r(c) > i) && (t.left += d + p +
						f)) : u > 0 && (s = t.left - e.collisionPosition
					.marginLeft + d + p + f - h, (s >
						0 || u > r(s)) && (t.left += d +
						p + f))
            },
            top: function (t, e) {
                var i, s, n = e.within,
					a = n.offset.top + n.scrollTop,
					o = n.height,
					h = n.isWindow ? n.scrollTop : n.offset
						.top,
					l = t.top - e.collisionPosition.marginTop,
					c = l - h,
					u = l + e.collisionHeight - o - h,
					d = "top" === e.my[1],
					p = d ? -e.elemHeight : "bottom" ===
						e.my[1] ? e.elemHeight : 0,
					f = "top" === e.at[1] ? e.targetHeight :
						"bottom" === e.at[1] ? -e.targetHeight :
						0,
					m = -2 * e.offset[1];
                0 > c ? (s = t.top + p + f + m + e
					.collisionHeight - o - a, t.top +
					p + f + m > c && (0 > s || r(c) >
						s) && (t.top += p + f + m)) : u >
					0 && (i = t.top - e.collisionPosition
						.marginTop + p + f + m - h, t.top +
						p + f + m > u && (i > 0 || u > r(
							i)) && (t.top += p + f + m))
            }
        },
        flipfit: {
            left: function () {
                t.ui.position.flip.left.apply(this,
					arguments), t.ui.position.fit.left
					.apply(this, arguments)
            },
            top: function () {
                t.ui.position.flip.top.apply(this,
					arguments), t.ui.position.fit.top
					.apply(this, arguments)
            }
        }
    },
	function () {
	    var e, i, s, n, a, o = document.getElementsByTagName(
				"body")[0],
			r = document.createElement("div");
	    e = document.createElement(o ? "div" :
			"body"), s = {
			    visibility: "hidden",
			    width: 0,
			    height: 0,
			    border: 0,
			    margin: 0,
			    background: "none"
			}, o && t.extend(s, {
			    position: "absolute",
			    left: "-1000px",
			    top: "-1000px"
			});
	    for (a in s) e.style[a] = s[a];
	    e.appendChild(r), i = o || document.documentElement,
			i.insertBefore(e, i.firstChild), r.style
			.cssText =
			"position: absolute; left: 10.7432222px;",
			n = t(r).offset().left, t.support.offsetFractions =
			n > 10 && 11 > n, e.innerHTML = "",
			i.removeChild(e)
	}()
})(jQuery);
(function (e) {
    e.widget("ui.draggable", e.ui.mouse, {
        version: "1.10.3",
        widgetEventPrefix: "drag",
        options: {
            addClasses: !0,
            appendTo: "parent",
            axis: !1,
            connectToSortable: !1,
            containment: !1,
            cursor: "auto",
            cursorAt: !1,
            grid: !1,
            handle: !1,
            helper: "original",
            iframeFix: !1,
            opacity: !1,
            refreshPositions: !1,
            revert: !1,
            revertDuration: 500,
            scope: "default",
            scroll: !0,
            scrollSensitivity: 20,
            scrollSpeed: 20,
            snap: !1,
            snapMode: "both",
            snapTolerance: 20,
            stack: !1,
            zIndex: !1,
            drag: null,
            start: null,
            stop: null
        },
        _create: function () {
            "original" !== this.options.helper ||
				/^(?:r|a|f)/.test(this.element.css(
					"position")) || (this.element[0]
					.style.position = "relative"),
				this.options.addClasses && this.element
				.addClass("ui-draggable"), this.options
				.disabled && this.element.addClass(
					"ui-draggable-disabled"), this._mouseInit()
        },
        _destroy: function () {
            this.element.removeClass(
				"ui-draggable ui-draggable-dragging ui-draggable-disabled"
			), this._mouseDestroy()
        },
        _mouseCapture: function (t) {
            var i = this.options;
            return this.helper || i.disabled ||
				e(t.target).closest(
					".ui-resizable-handle").length >
				0 ? !1 : (this.handle = this._getHandle(
					t), this.handle ? (e(i.iframeFix === !
					0 ? "iframe" : i.iframeFix).each(
					function () {
					    e(
							"<div class='ui-draggable-iframeFix' style='background: #fff;'></div>"
						).css({
						    width: this.offsetWidth +
								"px",
						    height: this.offsetHeight +
								"px",
						    position: "absolute",
						    opacity: "0.001",
						    zIndex: 1e3
						}).css(e(this).offset()).appendTo(
							"body")
					}), !0) : !1)
        },
        _mouseStart: function (t) {
            var i = this.options;
            return this.helper = this._createHelper(
				t), this.helper.addClass(
				"ui-draggable-dragging"), this._cacheHelperProportions(),
				e.ui.ddmanager && (e.ui.ddmanager
					.current = this), this._cacheMargins(),
				this.cssPosition = this.helper.css(
					"position"), this.scrollParent =
				this.helper.scrollParent(), this.offsetParent =
				this.helper.offsetParent(), this.offsetParentCssPosition =
				this.offsetParent.css("position"),
				this.offset = this.positionAbs =
				this.element.offset(), this.offset = {
				    top: this.offset.top - this.margins
						.top,
				    left: this.offset.left - this.margins
						.left
				}, this.offset.scroll = !1, e.extend(
				this.offset, {
				    click: {
				        left: t.pageX - this.offset.left,
				        top: t.pageY - this.offset.top
				    },
				    parent: this._getParentOffset(),
				    relative: this._getRelativeOffset()
				}), this.originalPosition = this.position =
				this._generatePosition(t), this.originalPageX =
				t.pageX, this.originalPageY = t.pageY,
				i.cursorAt && this._adjustOffsetFromHelper(
					i.cursorAt), this._setContainment(),
				this._trigger("start", t) === !1 ?
				(this._clear(), !1) : (this._cacheHelperProportions(),
					e.ui.ddmanager && !i.dropBehaviour &&
					e.ui.ddmanager.prepareOffsets(
						this, t), this._mouseDrag(t, !0),
					e.ui.ddmanager && e.ui.ddmanager
					.dragStart(this, t), !0)
        },
        _mouseDrag: function (t, i) {
            if ("fixed" === this.offsetParentCssPosition &&
				(this.offset.parent = this._getParentOffset()),
				this.position = this._generatePosition(
					t), this.positionAbs = this._convertPositionTo(
					"absolute"), !i) {
                var s = this._uiHash();
                if (this._trigger("drag", t, s) === !
					1) return this._mouseUp({}), !1;
                this.position = s.position
            }
            return this.options.axis && "y" ===
				this.options.axis || (this.helper[
						0].style.left = this.position.left +
					"px"), this.options.axis && "x" ===
				this.options.axis || (this.helper[
						0].style.top = this.position.top +
					"px"), e.ui.ddmanager && e.ui.ddmanager
				.drag(this, t), !1
        },
        _mouseStop: function (t) {
            var i = this,
				s = !1;
            return e.ui.ddmanager && !this.options
				.dropBehaviour && (s = e.ui.ddmanager
					.drop(this, t)), this.dropped &&
				(s = this.dropped, this.dropped = !
				1), "original" !== this.options.helper ||
				e.contains(this.element[0].ownerDocument,
					this.element[0]) ? ("invalid" ===
					this.options.revert && !s ||
					"valid" === this.options.revert &&
					s || this.options.revert === !0 ||
					e.isFunction(this.options.revert) &&
					this.options.revert.call(this.element,
						s) ? e(this.helper).animate(
						this.originalPosition, parseInt(
							this.options.revertDuration,
							10), function () {
							    i._trigger("stop", t) !== !1 &&
                                    i._clear()
							}) : this._trigger("stop", t) !== !
					1 && this._clear(), !1) : !1
        },
        _mouseUp: function (t) {
            return e(
				"div.ui-draggable-iframeFix").each(
				function () {
				    this.parentNode.removeChild(this)
				}), e.ui.ddmanager && e.ui.ddmanager
				.dragStop(this, t), e.ui.mouse.prototype
				._mouseUp.call(this, t)
        },
        cancel: function () {
            return this.helper.is(
				".ui-draggable-dragging") ? this._mouseUp({}) :
				this._clear(), this
        },
        _getHandle: function (t) {
            return this.options.handle ? !!e(
				t.target).closest(this.element.find(
				this.options.handle)).length : !0
        },
        _createHelper: function (t) {
            var i = this.options,
				s = e.isFunction(i.helper) ? e(i.helper
					.apply(this.element[0], [t])) :
					"clone" === i.helper ? this.element
					.clone().removeAttr("id") : this
					.element;
            return s.parents("body").length ||
				s.appendTo("parent" === i.appendTo ?
					this.element[0].parentNode : i.appendTo
			), s[0] === this.element[0] ||
				/(fixed|absolute)/.test(s.css(
					"position")) || s.css("position",
					"absolute"), s
        },
        _adjustOffsetFromHelper: function (
			t) {
            "string" == typeof t && (t = t.split(
				" ")), e.isArray(t) && (t = {
				    left: +t[0],
				    top: +t[1] || 0
				}), "left" in t && (this.offset.click
				.left = t.left + this.margins.left
			), "right" in t && (this.offset.click
				.left = this.helperProportions.width -
				t.right + this.margins.left),
				"top" in t && (this.offset.click.top =
					t.top + this.margins.top),
				"bottom" in t && (this.offset.click
					.top = this.helperProportions.height -
					t.bottom + this.margins.top)
        },
        _getParentOffset: function () {
            var t = this.offsetParent.offset();
            return "absolute" === this.cssPosition &&
				this.scrollParent[0] !== document &&
				e.contains(this.scrollParent[0],
					this.offsetParent[0]) && (t.left +=
					this.scrollParent.scrollLeft(),
					t.top += this.scrollParent.scrollTop()
			), (this.offsetParent[0] ===
				document.body || this.offsetParent[
					0].tagName && "html" === this.offsetParent[
					0].tagName.toLowerCase() && e.ui
				.ie) && (t = {
				    top: 0,
				    left: 0
				}), {
				    top: t.top + (parseInt(this.offsetParent
                            .css("borderTopWidth"), 10) ||
                        0),
				    left: t.left + (parseInt(this.offsetParent
                            .css("borderLeftWidth"), 10) ||
                        0)
				}
        },
        _getRelativeOffset: function () {
            if ("relative" === this.cssPosition) {
                var e = this.element.position();
                return {
                    top: e.top - (parseInt(this.helper
						.css("top"), 10) || 0) + this.scrollParent
						.scrollTop(),
                    left: e.left - (parseInt(this.helper
						.css("left"), 10) || 0) + this.scrollParent
						.scrollLeft()
                }
            }
            return {
                top: 0,
                left: 0
            }
        },
        _cacheMargins: function () {
            this.margins = {
                left: parseInt(this.element.css(
					"marginLeft"), 10) || 0,
                top: parseInt(this.element.css(
					"marginTop"), 10) || 0,
                right: parseInt(this.element.css(
					"marginRight"), 10) || 0,
                bottom: parseInt(this.element.css(
					"marginBottom"), 10) || 0
            }
        },
        _cacheHelperProportions: function () {
            this.helperProportions = {
                width: this.helper.outerWidth(),
                height: this.helper.outerHeight()
            }
        },
        _setContainment: function () {
            var t, i, s, n = this.options;
            return n.containment ? "window" ===
				n.containment ? (this.containment = [
					e(window).scrollLeft() - this.offset
					.relative.left - this.offset.parent
					.left, e(window).scrollTop() -
					this.offset.relative.top - this
					.offset.parent.top, e(window).scrollLeft() +
					e(window).width() - this.helperProportions
					.width - this.margins.left, e(
						window).scrollTop() + (e(
							window).height() || document.body
						.parentNode.scrollHeight) -
					this.helperProportions.height -
					this.margins.top
				], undefined) : "document" === n.containment ?
				(this.containment = [0, 0, e(
					document).width() - this.helperProportions
				.width - this.margins.left, (e(
						document).height() || document
					.body.parentNode.scrollHeight) -
				this.helperProportions.height -
				this.margins.top
				], undefined) : n.containment.constructor ===
				Array ? (this.containment = n.containment,
					undefined) : ("parent" === n.containment &&
					(n.containment = this.helper[0].parentNode),
					i = e(n.containment), s = i[0],
					s && (t = "hidden" !== i.css(
						"overflow"), this.containment = [
						(parseInt(i.css(
							"borderLeftWidth"), 10) || 0) +
						(parseInt(i.css("paddingLeft"),
							10) || 0), (parseInt(i.css(
							"borderTopWidth"), 10) || 0) +
						(parseInt(i.css("paddingTop"),
							10) || 0), (t ? Math.max(s.scrollWidth,
							s.offsetWidth) : s.offsetWidth) -
						(parseInt(i.css(
								"borderRightWidth"), 10) ||
							0) - (parseInt(i.css(
							"paddingRight"), 10) || 0) -
						this.helperProportions.width -
						this.margins.left - this.margins
						.right, (t ? Math.max(s.scrollHeight,
							s.offsetHeight) : s.offsetHeight) -
						(parseInt(i.css(
								"borderBottomWidth"), 10) ||
							0) - (parseInt(i.css(
							"paddingBottom"), 10) || 0) -
						this.helperProportions.height -
						this.margins.top - this.margins
						.bottom
						], this.relative_container = i),
					undefined) : (this.containment =
					null, undefined)
        },
        _convertPositionTo: function (t, i) {
            i || (i = this.position);
            var s = "absolute" === t ? 1 : -1,
				n = "absolute" !== this.cssPosition ||
					this.scrollParent[0] !==
					document && e.contains(this.scrollParent[
						0], this.offsetParent[0]) ?
					this.scrollParent : this.offsetParent;
            return this.offset.scroll || (this
				.offset.scroll = {
				    top: n.scrollTop(),
				    left: n.scrollLeft()
				}), {
				    top: i.top + this.offset.relative
                        .top * s + this.offset.parent.top *
                        s - ("fixed" === this.cssPosition ? -
                            this.scrollParent.scrollTop() :
                            this.offset.scroll.top) * s,
				    left: i.left + this.offset.relative
                        .left * s + this.offset.parent.left *
                        s - ("fixed" === this.cssPosition ? -
                            this.scrollParent.scrollLeft() :
                            this.offset.scroll.left) * s
				}
        },
        _generatePosition: function (t) {
            var i, s, n, a, o = this.options,
				r = "absolute" !== this.cssPosition ||
					this.scrollParent[0] !==
					document && e.contains(this.scrollParent[
						0], this.offsetParent[0]) ?
					this.scrollParent : this.offsetParent,
				h = t.pageX,
				l = t.pageY;
            return this.offset.scroll || (this
				.offset.scroll = {
				    top: r.scrollTop(),
				    left: r.scrollLeft()
				}), this.originalPosition && (
				this.containment && (this.relative_container ?
					(s = this.relative_container.offset(),
						i = [this.containment[0] + s.left,
							this.containment[1] + s.top,
							this.containment[2] + s.left,
							this.containment[3] + s.top
						]) : i = this.containment, t.pageX -
					this.offset.click.left < i[0] &&
					(h = i[0] + this.offset.click.left),
					t.pageY - this.offset.click.top <
					i[1] && (l = i[1] + this.offset.click
						.top), t.pageX - this.offset.click
					.left > i[2] && (h = i[2] + this
						.offset.click.left), t.pageY -
					this.offset.click.top > i[3] &&
					(l = i[3] + this.offset.click.top)
				), o.grid && (n = o.grid[1] ?
					this.originalPageY + Math.round(
						(l - this.originalPageY) / o.grid[
							1]) * o.grid[1] : this.originalPageY,
					l = i ? n - this.offset.click.top >=
					i[1] || n - this.offset.click.top >
					i[3] ? n : n - this.offset.click
					.top >= i[1] ? n - o.grid[1] : n +
					o.grid[1] : n, a = o.grid[0] ?
					this.originalPageX + Math.round(
						(h - this.originalPageX) / o.grid[
							0]) * o.grid[0] : this.originalPageX,
					h = i ? a - this.offset.click.left >=
					i[0] || a - this.offset.click.left >
					i[2] ? a : a - this.offset.click
					.left >= i[0] ? a - o.grid[0] :
					a + o.grid[0] : a)), {
					    top: l - this.offset.click.top -
                            this.offset.relative.top - this.offset
                            .parent.top + ("fixed" === this.cssPosition ? -
                                this.scrollParent.scrollTop() :
                                this.offset.scroll.top),
					    left: h - this.offset.click.left -
                            this.offset.relative.left - this
                            .offset.parent.left + ("fixed" ===
                                this.cssPosition ? -this.scrollParent
                                .scrollLeft() : this.offset.scroll
                                .left)
					}
        },
        _clear: function () {
            this.helper.removeClass(
				"ui-draggable-dragging"), this.helper[
				0] === this.element[0] || this.cancelHelperRemoval ||
				this.helper.remove(), this.helper =
				null, this.cancelHelperRemoval = !
				1
        },
        _trigger: function (t, i, s) {
            return s = s || this._uiHash(), e.ui
				.plugin.call(this, t, [i, s]),
				"drag" === t && (this.positionAbs =
					this._convertPositionTo(
						"absolute")), e.Widget.prototype
				._trigger.call(this, t, i, s)
        },
        plugins: {},
        _uiHash: function () {
            return {
                helper: this.helper,
                position: this.position,
                originalPosition: this.originalPosition,
                offset: this.positionAbs
            }
        }
    }), e.ui.plugin.add("draggable",
		"connectToSortable", {
		    start: function (t, i) {
		        var s = e(this).data(
					"ui-draggable"),
					n = s.options,
					a = e.extend({}, i, {
					    item: s.element
					});
		        s.sortables = [], e(n.connectToSortable)
					.each(function () {
					    var i = e.data(this,
							"ui-sortable");
					    i && !i.options.disabled && (s.sortables
							.push({
							    instance: i,
							    shouldRevert: i.options.revert
							}), i.refreshPositions(), i._trigger(
								"activate", t, a))
					})
		    },
		    stop: function (t, i) {
		        var s = e(this).data(
					"ui-draggable"),
					n = e.extend({}, i, {
					    item: s.element
					});
		        e.each(s.sortables, function () {
		            this.instance.isOver ? (this.instance
						.isOver = 0, s.cancelHelperRemoval = !
						0, this.instance.cancelHelperRemoval = !
						1, this.shouldRevert && (this.instance
							.options.revert = this.shouldRevert
						), this.instance._mouseStop(t),
						this.instance.options.helper =
						this.instance.options._helper,
						"original" === s.options.helper &&
						this.instance.currentItem.css({
						    top: "auto",
						    left: "auto"
						})) : (this.instance.cancelHelperRemoval = !
						1, this.instance._trigger(
							"deactivate", t, n))
		        })
		    },
		    drag: function (t, i) {
		        var s = e(this).data(
					"ui-draggable"),
					n = this;
		        e.each(s.sortables, function () {
		            var a = !1,
						o = this;
		            this.instance.positionAbs = s.positionAbs,
						this.instance.helperProportions =
						s.helperProportions, this.instance
						.offset.click = s.offset.click,
						this.instance._intersectsWith(
							this.instance.containerCache) &&
						(a = !0, e.each(s.sortables,
						function () {
						    return this.instance.positionAbs =
								s.positionAbs, this.instance
								.helperProportions = s.helperProportions,
								this.instance.offset.click =
								s.offset.click, this !== o &&
								this.instance._intersectsWith(
									this.instance.containerCache
							) && e.contains(o.instance.element[
								0], this.instance.element[0]) &&
								(a = !1), a
						})), a ? (this.instance.isOver ||
							(this.instance.isOver = 1,
								this.instance.currentItem = e(
									n).clone().removeAttr("id").appendTo(
									this.instance.element).data(
									"ui-sortable-item", !0),
								this.instance.options._helper =
								this.instance.options.helper,
								this.instance.options.helper =
								function () {
								    return i.helper[0]
								}, t.target = this.instance.currentItem[
									0], this.instance._mouseCapture(
									t, !0), this.instance._mouseStart(
									t, !0, !0), this.instance.offset
								.click.top = s.offset.click.top,
								this.instance.offset.click.left =
								s.offset.click.left, this.instance
								.offset.parent.left -= s.offset
								.parent.left - this.instance.offset
								.parent.left, this.instance.offset
								.parent.top -= s.offset.parent
								.top - this.instance.offset.parent
								.top, s._trigger("toSortable",
									t), s.dropped = this.instance
								.element, s.currentItem = s.element,
								this.instance.fromOutside = s
							), this.instance.currentItem &&
							this.instance._mouseDrag(t)) :
						this.instance.isOver && (this.instance
							.isOver = 0, this.instance.cancelHelperRemoval = !
							0, this.instance.options.revert = !
							1, this.instance._trigger(
								"out", t, this.instance._uiHash(
									this.instance)), this.instance
							._mouseStop(t, !0), this.instance
							.options.helper = this.instance
							.options._helper, this.instance
							.currentItem.remove(), this.instance
							.placeholder && this.instance.placeholder
							.remove(), s._trigger(
								"fromSortable", t), s.dropped = !
							1)
		        })
		    }
		}), e.ui.plugin.add("draggable",
		"cursor", {
		    start: function () {
		        var t = e("body"),
					i = e(this).data("ui-draggable").options;
		        t.css("cursor") && (i._cursor = t.css(
					"cursor")), t.css("cursor", i.cursor)
		    },
		    stop: function () {
		        var t = e(this).data(
					"ui-draggable").options;
		        t._cursor && e("body").css(
					"cursor", t._cursor)
		    }
		}), e.ui.plugin.add("draggable",
		"opacity", {
		    start: function (t, i) {
		        var s = e(i.helper),
					n = e(this).data("ui-draggable").options;
		        s.css("opacity") && (n._opacity =
					s.css("opacity")), s.css(
					"opacity", n.opacity)
		    },
		    stop: function (t, i) {
		        var s = e(this).data(
					"ui-draggable").options;
		        s._opacity && e(i.helper).css(
					"opacity", s._opacity)
		    }
		}), e.ui.plugin.add("draggable",
		"scroll", {
		    start: function () {
		        var t = e(this).data(
					"ui-draggable");
		        t.scrollParent[0] !== document &&
					"HTML" !== t.scrollParent[0].tagName &&
					(t.overflowOffset = t.scrollParent
					.offset())
		    },
		    drag: function (t) {
		        var i = e(this).data(
					"ui-draggable"),
					s = i.options,
					n = !1;
		        i.scrollParent[0] !== document &&
					"HTML" !== i.scrollParent[0].tagName ?
					(s.axis && "x" === s.axis || (i.overflowOffset
						.top + i.scrollParent[0].offsetHeight -
						t.pageY < s.scrollSensitivity ?
						i.scrollParent[0].scrollTop = n =
						i.scrollParent[0].scrollTop + s.scrollSpeed :
						t.pageY - i.overflowOffset.top <
						s.scrollSensitivity && (i.scrollParent[
							0].scrollTop = n = i.scrollParent[
							0].scrollTop - s.scrollSpeed)),
					s.axis && "y" === s.axis || (i.overflowOffset
						.left + i.scrollParent[0].offsetWidth -
						t.pageX < s.scrollSensitivity ?
						i.scrollParent[0].scrollLeft = n =
						i.scrollParent[0].scrollLeft + s
						.scrollSpeed : t.pageX - i.overflowOffset
						.left < s.scrollSensitivity && (
							i.scrollParent[0].scrollLeft =
							n = i.scrollParent[0].scrollLeft -
							s.scrollSpeed))) : (s.axis &&
						"x" === s.axis || (t.pageY - e(
								document).scrollTop() < s.scrollSensitivity ?
							n = e(document).scrollTop(e(
								document).scrollTop() - s.scrollSpeed) :
							e(window).height() - (t.pageY -
								e(document).scrollTop()) < s.scrollSensitivity &&
							(n = e(document).scrollTop(e(
								document).scrollTop() + s.scrollSpeed))
						), s.axis && "y" === s.axis || (
							t.pageX - e(document).scrollLeft() <
							s.scrollSensitivity ? n = e(
								document).scrollLeft(e(
								document).scrollLeft() - s.scrollSpeed) :
							e(window).width() - (t.pageX -
								e(document).scrollLeft()) < s.scrollSensitivity &&
							(n = e(document).scrollLeft(e(
								document).scrollLeft() + s.scrollSpeed))
						)), n !== !1 && e.ui.ddmanager && !
					s.dropBehaviour && e.ui.ddmanager
					.prepareOffsets(i, t)
		    }
		}), e.ui.plugin.add("draggable",
		"snap", {
		    start: function () {
		        var t = e(this).data(
					"ui-draggable"),
					i = t.options;
		        t.snapElements = [], e(i.snap.constructor !==
					String ? i.snap.items ||
					":data(ui-draggable)" : i.snap).each(
					function () {
					    var i = e(this),
							s = i.offset();
					    this !== t.element[0] && t.snapElements
							.push({
							    item: this,
							    width: i.outerWidth(),
							    height: i.outerHeight(),
							    top: s.top,
							    left: s.left
							})
					})
		    },
		    drag: function (t, i) {
		        var s, n, a, o, r, h, l, u, c, d,
						p = e(this).data("ui-draggable"),
					f = p.options,
					m = f.snapTolerance,
					g = i.offset.left,
					v = g + p.helperProportions.width,
					b = i.offset.top,
					y = b + p.helperProportions.height;
		        for (c = p.snapElements.length - 1; c >=
					0; c--) r = p.snapElements[c].left,
					h = r + p.snapElements[c].width,
					l = p.snapElements[c].top, u = l +
					p.snapElements[c].height, r - m >
					v || g > h + m || l - m > y || b >
					u + m || !e.contains(p.snapElements[
						c].item.ownerDocument, p.snapElements[
						c].item) ? (p.snapElements[c].snapping &&
						p.options.snap.release && p.options
						.snap.release.call(p.element, t,
							e.extend(p._uiHash(), {
							    snapItem: p.snapElements[c].item
							})), p.snapElements[c].snapping = !
						1) : ("inner" !== f.snapMode &&
						(s = m >= Math.abs(l - y), n = m >=
							Math.abs(u - b), a = m >= Math.abs(
								r - v), o = m >= Math.abs(h -
								g), s && (i.position.top = p._convertPositionTo(
								"relative", {
								    top: l - p.helperProportions
										.height,
								    left: 0
								}).top - p.margins.top), n &&
							(i.position.top = p._convertPositionTo(
								"relative", {
								    top: u,
								    left: 0
								}).top - p.margins.top), a &&
							(i.position.left = p._convertPositionTo(
								"relative", {
								    top: 0,
								    left: r - p.helperProportions
										.width
								}).left - p.margins.left), o &&
							(i.position.left = p._convertPositionTo(
								"relative", {
								    top: 0,
								    left: h
								}).left - p.margins.left)), d =
						s || n || a || o, "outer" !== f.snapMode &&
						(s = m >= Math.abs(l - b), n = m >=
							Math.abs(u - y), a = m >= Math.abs(
								r - g), o = m >= Math.abs(h -
								v), s && (i.position.top = p._convertPositionTo(
								"relative", {
								    top: l,
								    left: 0
								}).top - p.margins.top), n &&
							(i.position.top = p._convertPositionTo(
								"relative", {
								    top: u - p.helperProportions
										.height,
								    left: 0
								}).top - p.margins.top), a &&
							(i.position.left = p._convertPositionTo(
								"relative", {
								    top: 0,
								    left: r
								}).left - p.margins.left), o &&
							(i.position.left = p._convertPositionTo(
								"relative", {
								    top: 0,
								    left: h - p.helperProportions
										.width
								}).left - p.margins.left)), !p
						.snapElements[c].snapping && (s ||
							n || a || o || d) && p.options.snap
						.snap && p.options.snap.snap.call(
							p.element, t, e.extend(p._uiHash(), {
							    snapItem: p.snapElements[c].item
							})), p.snapElements[c].snapping =
						s || n || a || o || d)
		    }
		}), e.ui.plugin.add("draggable",
		"stack", {
		    start: function () {
		        var t, i = this.data(
						"ui-draggable").options,
					s = e.makeArray(e(i.stack)).sort(
						function (t, i) {
						    return (parseInt(e(t).css(
								"zIndex"), 10) || 0) - (
								parseInt(e(i).css("zIndex"),
									10) || 0)
						});
		        s.length && (t = parseInt(e(s[0]).css(
					"zIndex"), 10) || 0, e(s).each(
					function (i) {
					    e(this).css("zIndex", t + i)
					}), this.css("zIndex", t + s.length))
		    }
		}), e.ui.plugin.add("draggable",
		"zIndex", {
		    start: function (t, i) {
		        var s = e(i.helper),
					n = e(this).data("ui-draggable").options;
		        s.css("zIndex") && (n._zIndex = s.css(
					"zIndex")), s.css("zIndex", n.zIndex)
		    },
		    stop: function (t, i) {
		        var s = e(this).data(
					"ui-draggable").options;
		        s._zIndex && e(i.helper).css(
					"zIndex", s._zIndex)
		    }
		})
})(jQuery);
(function (e) {
    function t(e, t, i) {
        return e > t && t + i > e
    }
    e.widget("ui.droppable", {
        version: "1.10.3",
        widgetEventPrefix: "drop",
        options: {
            accept: "*",
            activeClass: !1,
            addClasses: !0,
            greedy: !1,
            hoverClass: !1,
            scope: "default",
            tolerance: "intersect",
            activate: null,
            deactivate: null,
            drop: null,
            out: null,
            over: null
        },
        _create: function () {
            var t = this.options,
				i = t.accept;
            this.isover = !1, this.isout = !0,
				this.accept = e.isFunction(i) ? i :
				function (e) {
				    return e.is(i)
				}, this.proportions = {
				    width: this.element[0].offsetWidth,
				    height: this.element[0].offsetHeight
				}, e.ui.ddmanager.droppables[t.scope] =
				e.ui.ddmanager.droppables[t.scope] || [],
				e.ui.ddmanager.droppables[t.scope]
				.push(this), t.addClasses && this
				.element.addClass("ui-droppable")
        },
        _destroy: function () {
            for (var t = 0, i = e.ui.ddmanager
					.droppables[this.options.scope]; i
				.length > t; t++) i[t] === this &&
				i.splice(t, 1);
            this.element.removeClass(
				"ui-droppable ui-droppable-disabled"
			)
        },
        _setOption: function (t, i) {
            "accept" === t && (this.accept = e
				.isFunction(i) ? i : function (e) {
				    return e.is(i)
				}), e.Widget.prototype._setOption
				.apply(this, arguments)
        },
        _activate: function (t) {
            var i = e.ui.ddmanager.current;
            this.options.activeClass && this.element
				.addClass(this.options.activeClass),
				i && this._trigger("activate", t,
					this.ui(i))
        },
        _deactivate: function (t) {
            var i = e.ui.ddmanager.current;
            this.options.activeClass && this.element
				.removeClass(this.options.activeClass),
				i && this._trigger("deactivate",
					t, this.ui(i))
        },
        _over: function (t) {
            var i = e.ui.ddmanager.current;
            i && (i.currentItem || i.element)[
				0] !== this.element[0] && this.accept
				.call(this.element[0], i.currentItem ||
					i.element) && (this.options.hoverClass &&
					this.element.addClass(this.options
						.hoverClass), this._trigger(
						"over", t, this.ui(i)))
        },
        _out: function (t) {
            var i = e.ui.ddmanager.current;
            i && (i.currentItem || i.element)[
				0] !== this.element[0] && this.accept
				.call(this.element[0], i.currentItem ||
					i.element) && (this.options.hoverClass &&
					this.element.removeClass(this.options
						.hoverClass), this._trigger(
						"out", t, this.ui(i)))
        },
        _drop: function (t, i) {
            var s = i || e.ui.ddmanager.current,
				n = !1;
            return s && (s.currentItem || s.element)[
				0] !== this.element[0] ? (this.element
				.find(":data(ui-droppable)").not(
					".ui-draggable-dragging").each(
					function () {
					    var t = e.data(this,
							"ui-droppable");
					    return t.options.greedy && !t.options
							.disabled && t.options.scope ===
							s.options.scope && t.accept.call(
								t.element[0], s.currentItem ||
								s.element) && e.ui.intersect(
								s, e.extend(t, {
								    offset: t.element.offset()
								}), t.options.tolerance) ? (n = !
								0, !1) : undefined
					}), n ? !1 : this.accept.call(
					this.element[0], s.currentItem ||
					s.element) ? (this.options.activeClass &&
					this.element.removeClass(this.options
						.activeClass), this.options.hoverClass &&
					this.element.removeClass(this.options
						.hoverClass), this._trigger(
						"drop", t, this.ui(s)), this.element
				) : !1) : !1
        },
        ui: function (e) {
            return {
                draggable: e.currentItem || e.element,
                helper: e.helper,
                position: e.position,
                offset: e.positionAbs
            }
        }
    }), e.ui.intersect = function (e, i,
		s) {
        if (!i.offset) return !1;
        var n, a, o = (e.positionAbs || e.position
				.absolute).left,
			r = o + e.helperProportions.width,
			h = (e.positionAbs || e.position.absolute)
				.top,
			l = h + e.helperProportions.height,
			u = i.offset.left,
			c = u + i.proportions.width,
			d = i.offset.top,
			p = d + i.proportions.height;
        switch (s) {
            case "fit":
                return o >= u && c >= r && h >= d &&
                    p >= l;
            case "intersect":
                return o + e.helperProportions.width /
                    2 > u && c > r - e.helperProportions
                    .width / 2 && h + e.helperProportions
                    .height / 2 > d && p > l - e.helperProportions
                    .height / 2;
            case "pointer":
                return n = (e.positionAbs || e.position
                    .absolute).left + (e.clickOffset ||
                    e.offset.click).left, a = (e.positionAbs ||
                    e.position.absolute).top + (e.clickOffset ||
                    e.offset.click).top, t(a, d, i.proportions
                    .height) && t(n, u, i.proportions.width);
            case "touch":
                return (h >= d && p >= h || l >= d &&
                    p >= l || d > h && l > p) && (o >=
                    u && c >= o || r >= u && c >= r ||
                    u > o && r > c);
            default:
                return !1
        }
    }, e.ui.ddmanager = {
        current: null,
        droppables: {
            "default": []
        },
        prepareOffsets: function (t, i) {
            var s, n, a = e.ui.ddmanager.droppables[
					t.options.scope] || [],
				o = i ? i.type : null,
				r = (t.currentItem || t.element).find(
					":data(ui-droppable)").addBack();
            e: for (s = 0; a.length > s; s++)
                if (!(a[s].options.disabled || t && !
					a[s].accept.call(a[s].element[0],
						t.currentItem || t.element))) {
                    for (n = 0; r.length > n; n++)
                        if (r[n] === a[s].element[0]) {
                            a[s].proportions.height = 0;
                            continue e
                        }
                    a[s].visible = "none" !== a[s].element
						.css("display"), a[s].visible &&
						("mousedown" === o && a[s]._activate
						.call(a[s], i), a[s].offset = a[
							s].element.offset(), a[s].proportions = {
							    width: a[s].element[0].offsetWidth,
							    height: a[s].element[0].offsetHeight
							})
                }
        },
        drop: function (t, i) {
            var s = !1;
            return e.each((e.ui.ddmanager.droppables[
					t.options.scope] || []).slice(),
				function () {
				    this.options && (!this.options.disabled &&
						this.visible && e.ui.intersect(t,
							this, this.options.tolerance) &&
						(s = this._drop.call(this, i) ||
							s), !this.options.disabled &&
						this.visible && this.accept.call(
							this.element[0], t.currentItem ||
							t.element) && (this.isout = !0,
							this.isover = !1, this._deactivate
							.call(this, i)))
				}), s
        },
        dragStart: function (t, i) {
            t.element.parentsUntil("body").bind(
				"scroll.droppable", function () {
				    t.options.refreshPositions || e.ui
						.ddmanager.prepareOffsets(t, i)
				})
        },
        drag: function (t, i) {
            t.options.refreshPositions && e.ui.ddmanager
				.prepareOffsets(t, i), e.each(e.ui
					.ddmanager.droppables[t.options.scope] || [],
					function () {
					    if (!this.options.disabled && !
							this.greedyChild && this.visible
						) {
					        var s, n, a, o = e.ui.intersect(
									t, this, this.options.tolerance
								),
								r = !o && this.isover ?
									"isout" : o && !this.isover ?
									"isover" : null;
					        r && (this.options.greedy && (n =
									this.options.scope, a = this.element
									.parents(
										":data(ui-droppable)").filter(
										function () {
										    return e.data(this,
												"ui-droppable").options.scope ===
												n
										}), a.length && (s = e.data(
											a[0], "ui-droppable"), s.greedyChild =
										"isover" === r)), s &&
								"isover" === r && (s.isover = !
									1, s.isout = !0, s._out.call(
										s, i)), this[r] = !0, this[
									"isout" === r ? "isover" :
									"isout"] = !1, this["isover" ===
									r ? "_over" : "_out"].call(
									this, i), s && "isout" === r &&
								(s.isout = !1, s.isover = !0,
									s._over.call(s, i)))
					    }
					})
        },
        dragStop: function (t, i) {
            t.element.parentsUntil("body").unbind(
				"scroll.droppable"), t.options.refreshPositions ||
				e.ui.ddmanager.prepareOffsets(t, i)
        }
    }
})(jQuery);
(function (e) {
    function t(e) {
        return parseInt(e, 10) || 0
    }

    function i(e) {
        return !isNaN(parseInt(e, 10))
    }
    e.widget("ui.resizable", e.ui.mouse, {
        version: "1.10.3",
        widgetEventPrefix: "resize",
        options: {
            alsoResize: !1,
            animate: !1,
            animateDuration: "slow",
            animateEasing: "swing",
            aspectRatio: !1,
            autoHide: !1,
            containment: !1,
            ghost: !1,
            grid: !1,
            handles: "e,s,se",
            helper: !1,
            maxHeight: null,
            maxWidth: null,
            minHeight: 10,
            minWidth: 10,
            zIndex: 90,
            resize: null,
            start: null,
            stop: null
        },
        _create: function () {
            var t, i, s, n, a, o = this,
				r = this.options;
            if (this.element.addClass(
					"ui-resizable"), e.extend(this, {
                _aspectRatio: !!r.aspectRatio,
                aspectRatio: r.aspectRatio,
                originalElement: this.element,
                _proportionallyResizeElements: [],
                _helper: r.helper || r.ghost ||
						r.animate ? r.helper ||
						"ui-resizable-helper" : null
            }), this.element[0].nodeName.match(
					/canvas|textarea|input|select|button|img/i
				) && (this.element.wrap(e(
						"<div class='ui-wrapper' style='overflow: hidden;'></div>"
					).css({
                position: this.element.css(
							"position"),
                width: this.element.outerWidth(),
                height: this.element.outerHeight(),
                top: this.element.css("top"),
                left: this.element.css("left")
            })), this.element = this.element
					.parent().data("ui-resizable",
						this.element.data(
							"ui-resizable")), this.elementIsWrapper = !
					0, this.element.css({
                marginLeft: this.originalElement
							.css("marginLeft"),
                marginTop: this.originalElement
							.css("marginTop"),
                marginRight: this.originalElement
							.css("marginRight"),
                marginBottom: this.originalElement
							.css("marginBottom")
            }), this.originalElement.css({
                marginLeft: 0,
                marginTop: 0,
                marginRight: 0,
                marginBottom: 0
            }), this.originalResizeStyle =
					this.originalElement.css(
						"resize"), this.originalElement
					.css("resize", "none"), this._proportionallyResizeElements
					.push(this.originalElement.css({
                position: "static",
                zoom: 1,
                display: "block"
            })), this.originalElement.css({
                margin: this.originalElement.css(
							"margin")
            }), this._proportionallyResize()
				), this.handles = r.handles || (e(
					".ui-resizable-handle", this.element
				).length ? {
                n: ".ui-resizable-n",
                e: ".ui-resizable-e",
                s: ".ui-resizable-s",
                w: ".ui-resizable-w",
                se: ".ui-resizable-se",
                sw: ".ui-resizable-sw",
                ne: ".ui-resizable-ne",
                nw: ".ui-resizable-nw"
            } : "e,s,se"), this.handles.constructor ===
				String)
                for ("all" === this.handles && (
						this.handles =
						"n,e,s,w,se,sw,ne,nw"), t =
					this.handles.split(","), this.handles = {},
					i = 0; t.length > i; i++) s = e.trim(
					t[i]), a = "ui-resizable-" + s,
					n = e(
						"<div class='ui-resizable-handle " +
						a + "'></div>"), n.css({
						    zIndex: r.zIndex
						}), "se" === s && n.addClass(
						"ui-icon ui-icon-gripsmall-diagonal-se"
				), this.handles[s] =
					".ui-resizable-" + s, this.element
					.append(n);
            this._renderAxis = function (t) {
                var i, s, n, a;
                t = t || this.element;
                for (i in this.handles) this.handles[
					i].constructor === String && (
					this.handles[i] = e(this.handles[
						i], this.element).show()), this
					.elementIsWrapper && this.originalElement[
						0].nodeName.match(
						/textarea|input|select|button/i
				) && (s = e(this.handles[i], this
						.element), a =
					/sw|ne|nw|se|n|s/.test(i) ? s.outerHeight() :
					s.outerWidth(), n = ["padding",
						/ne|nw|n/.test(i) ? "Top" :
						/se|sw|s/.test(i) ? "Bottom" :
						/^e$/.test(i) ? "Right" :
						"Left"
					].join(""), t.css(n, a), this._proportionallyResize()
				), e(this.handles[i]).length
            }, this._renderAxis(this.element),
				this._handles = e(
					".ui-resizable-handle", this.element
			).disableSelection(), this._handles
				.mouseover(function () {
				    o.resizing || (this.className &&
						(n = this.className.match(
							/ui-resizable-(se|sw|ne|nw|n|e|s|w)/i
						)), o.axis = n && n[1] ? n[1] :
						"se")
				}), r.autoHide && (this._handles.hide(),
					e(this.element).addClass(
						"ui-resizable-autohide").mouseenter(
						function () {
						    r.disabled || (e(this).removeClass(
									"ui-resizable-autohide"), o._handles
								.show())
						}).mouseleave(function () {
						    r.disabled || o.resizing || (e(
                                    this).addClass(
                                    "ui-resizable-autohide"), o._handles
                                .hide())
						})), this._mouseInit()
        },
        _destroy: function () {
            this._mouseDestroy();
            var t, i = function (t) {
                e(t).removeClass(
                    "ui-resizable ui-resizable-disabled ui-resizable-resizing"
                ).removeData("resizable").removeData(
                    "ui-resizable").unbind(
                    ".resizable").find(
                    ".ui-resizable-handle").remove()
            };
            return this.elementIsWrapper && (i(
					this.element), t = this.element,
				this.originalElement.css({
				    position: t.css("position"),
				    width: t.outerWidth(),
				    height: t.outerHeight(),
				    top: t.css("top"),
				    left: t.css("left")
				}).insertAfter(t), t.remove()),
				this.originalElement.css("resize",
					this.originalResizeStyle), i(
					this.originalElement), this
        },
        _mouseCapture: function (t) {
            var i, s, n = !1;
            for (i in this.handles) s = e(this
				.handles[i])[0], (s === t.target ||
				e.contains(s, t.target)) && (n = !
				0);
            return !this.options.disabled && n
        },
        _mouseStart: function (i) {
            var s, n, a, o = this.options,
				r = this.element.position(),
				h = this.element;
            return this.resizing = !0,
				/absolute/.test(h.css("position")) ?
				h.css({
				    position: "absolute",
				    top: h.css("top"),
				    left: h.css("left")
				}) : h.is(".ui-draggable") && h.css({
				    position: "absolute",
				    top: r.top,
				    left: r.left
				}), this._renderProxy(), s = t(
					this.helper.css("left")), n = t(
					this.helper.css("top")), o.containment &&
				(s += e(o.containment).scrollLeft() ||
				0, n += e(o.containment).scrollTop() ||
				0), this.offset = this.helper.offset(),
				this.position = {
				    left: s,
				    top: n
				}, this.size = this._helper ? {
				    width: h.outerWidth(),
				    height: h.outerHeight()
				} : {
				    width: h.width(),
				    height: h.height()
				}, this.originalSize = this._helper ? {
				    width: h.outerWidth(),
				    height: h.outerHeight()
				} : {
				    width: h.width(),
				    height: h.height()
				}, this.originalPosition = {
				    left: s,
				    top: n
				}, this.sizeDiff = {
				    width: h.outerWidth() - h.width(),
				    height: h.outerHeight() - h.height()
				}, this.originalMousePosition = {
				    left: i.pageX,
				    top: i.pageY
				}, this.aspectRatio = "number" ==
				typeof o.aspectRatio ? o.aspectRatio :
				this.originalSize.width / this.originalSize
				.height || 1, a = e(
					".ui-resizable-" + this.axis).css(
					"cursor"), e("body").css(
					"cursor", "auto" === a ? this.axis +
					"-resize" : a), h.addClass(
					"ui-resizable-resizing"), this._propagate(
					"start", i), !0
        },
        _mouseDrag: function (t) {
            var i, s = this.helper,
				n = {}, a = this.originalMousePosition,
				o = this.axis,
				r = this.position.top,
				h = this.position.left,
				l = this.size.width,
				u = this.size.height,
				c = t.pageX - a.left || 0,
				d = t.pageY - a.top || 0,
				p = this._change[o];
            return p ? (i = p.apply(this, [t,
					c, d
            ]), this._updateVirtualBoundaries(
					t.shiftKey), (this._aspectRatio ||
					t.shiftKey) && (i = this._updateRatio(
					i, t)), i = this._respectSize(i,
					t), this._updateCache(i), this._propagate(
					"resize", t), this.position.top !==
				r && (n.top = this.position.top +
					"px"), this.position.left !== h &&
				(n.left = this.position.left +
					"px"), this.size.width !== l &&
				(n.width = this.size.width + "px"),
				this.size.height !== u && (n.height =
					this.size.height + "px"), s.css(
					n), !this._helper && this._proportionallyResizeElements
				.length && this._proportionallyResize(),
				e.isEmptyObject(n) || this._trigger(
					"resize", t, this.ui()), !1) : !
				1
        },
        _mouseStop: function (t) {
            this.resizing = !1;
            var i, s, n, a, o, r, h, l = this.options,
				u = this;
            return this._helper && (i = this._proportionallyResizeElements,
				s = i.length && /textarea/i.test(
					i[0].nodeName), n = s && e.ui.hasScroll(
					i[0], "left") ? 0 : u.sizeDiff.height,
				a = s ? 0 : u.sizeDiff.width, o = {
				    width: u.helper.width() - a,
				    height: u.helper.height() - n
				}, r = parseInt(u.element.css(
					"left"), 10) + (u.position.left -
					u.originalPosition.left) || null,
				h = parseInt(u.element.css("top"),
					10) + (u.position.top - u.originalPosition
					.top) || null, l.animate || this
				.element.css(e.extend(o, {
				    top: h,
				    left: r
				})), u.helper.height(u.size.height),
				u.helper.width(u.size.width),
				this._helper && !l.animate &&
				this._proportionallyResize()), e(
				"body").css("cursor", "auto"),
				this.element.removeClass(
					"ui-resizable-resizing"), this._propagate(
					"stop", t), this._helper && this
				.helper.remove(), !1
        },
        _updateVirtualBoundaries: function (
			e) {
            var t, s, n, a, o, r = this.options;
            o = {
                minWidth: i(r.minWidth) ? r.minWidth : 0,
                maxWidth: i(r.maxWidth) ? r.maxWidth : 1 /
					0,
                minHeight: i(r.minHeight) ? r.minHeight : 0,
                maxHeight: i(r.maxHeight) ? r.maxHeight : 1 /
					0
            }, (this._aspectRatio || e) && (t =
				o.minHeight * this.aspectRatio, n =
				o.minWidth / this.aspectRatio, s =
				o.maxHeight * this.aspectRatio, a =
				o.maxWidth / this.aspectRatio, t >
				o.minWidth && (o.minWidth = t), n >
				o.minHeight && (o.minHeight = n),
				o.maxWidth > s && (o.maxWidth = s),
				o.maxHeight > a && (o.maxHeight =
					a)), this._vBoundaries = o
        },
        _updateCache: function (e) {
            this.offset = this.helper.offset(),
				i(e.left) && (this.position.left =
					e.left), i(e.top) && (this.position
					.top = e.top), i(e.height) && (
					this.size.height = e.height), i(
					e.width) && (this.size.width = e
					.width)
        },
        _updateRatio: function (e) {
            var t = this.position,
				s = this.size,
				n = this.axis;
            return i(e.height) ? e.width = e.height *
				this.aspectRatio : i(e.width) &&
				(e.height = e.width / this.aspectRatio),
				"sw" === n && (e.left = t.left +
					(s.width - e.width), e.top =
					null), "nw" === n && (e.top = t.top +
					(s.height - e.height), e.left =
					t.left + (s.width - e.width)), e
        },
        _respectSize: function (e) {
            var t = this._vBoundaries,
				s = this.axis,
				n = i(e.width) && t.maxWidth && t
					.maxWidth < e.width,
				a = i(e.height) && t.maxHeight &&
					t.maxHeight < e.height,
				o = i(e.width) && t.minWidth && t
					.minWidth > e.width,
				r = i(e.height) && t.minHeight &&
					t.minHeight > e.height,
				h = this.originalPosition.left +
					this.originalSize.width,
				l = this.position.top + this.size
					.height,
				u = /sw|nw|w/.test(s),
				c = /nw|ne|n/.test(s);
            return o && (e.width = t.minWidth),
				r && (e.height = t.minHeight), n &&
				(e.width = t.maxWidth), a && (e.height =
					t.maxHeight), o && u && (e.left =
					h - t.minWidth), n && u && (e.left =
					h - t.maxWidth), r && c && (e.top =
					l - t.minHeight), a && c && (e.top =
					l - t.maxHeight), e.width || e.height ||
				e.left || !e.top ? e.width || e.height ||
				e.top || !e.left || (e.left =
					null) : e.top = null, e
        },
        _proportionallyResize: function () {
            if (this._proportionallyResizeElements
				.length) {
                var e, t, i, s, n, a = this.helper ||
						this.element;
                for (e = 0; this._proportionallyResizeElements
					.length > e; e++) {
                    if (n = this._proportionallyResizeElements[
						e], !this.borderDif)
                        for (this.borderDif = [], i = [
							n.css("borderTopWidth"), n.css(
								"borderRightWidth"), n.css(
								"borderBottomWidth"), n.css(
								"borderLeftWidth")
                        ], s = [n.css("paddingTop"), n
							.css("paddingRight"), n.css(
								"paddingBottom"), n.css(
								"paddingLeft")
                        ], t = 0; i.length > t; t++)
                            this.borderDif[t] = (parseInt(
								i[t], 10) || 0) + (parseInt(s[
								t], 10) || 0);
                    n.css({
                        height: a.height() - this.borderDif[
							0] - this.borderDif[2] || 0,
                        width: a.width() - this.borderDif[
							1] - this.borderDif[3] || 0
                    })
                }
            }
        },
        _renderProxy: function () {
            var t = this.element,
				i = this.options;
            this.elementOffset = t.offset(),
				this._helper ? (this.helper =
					this.helper || e(
						"<div style='overflow:hidden;'></div>"
					), this.helper.addClass(this._helper)
					.css({
					    width: this.element.outerWidth() -
							1,
					    height: this.element.outerHeight() -
							1,
					    position: "absolute",
					    left: this.elementOffset.left +
							"px",
					    top: this.elementOffset.top +
							"px",
					    zIndex: ++i.zIndex
					}), this.helper.appendTo("body")
					.disableSelection()) : this.helper =
				this.element
        },
        _change: {
            e: function (e, t) {
                return {
                    width: this.originalSize.width +
						t
                }
            },
            w: function (e, t) {
                var i = this.originalSize,
					s = this.originalPosition;
                return {
                    left: s.left + t,
                    width: i.width - t
                }
            },
            n: function (e, t, i) {
                var s = this.originalSize,
					n = this.originalPosition;
                return {
                    top: n.top + i,
                    height: s.height - i
                }
            },
            s: function (e, t, i) {
                return {
                    height: this.originalSize.height +
						i
                }
            },
            se: function (t, i, s) {
                return e.extend(this._change.s.apply(
						this, arguments), this._change.e
					.apply(this, [t, i, s]))
            },
            sw: function (t, i, s) {
                return e.extend(this._change.s.apply(
						this, arguments), this._change.w
					.apply(this, [t, i, s]))
            },
            ne: function (t, i, s) {
                return e.extend(this._change.n.apply(
						this, arguments), this._change.e
					.apply(this, [t, i, s]))
            },
            nw: function (t, i, s) {
                return e.extend(this._change.n.apply(
						this, arguments), this._change.w
					.apply(this, [t, i, s]))
            }
        },
        _propagate: function (t, i) {
            e.ui.plugin.call(this, t, [i, this
				.ui()
            ]), "resize" !== t && this._trigger(
				t, i, this.ui())
        },
        plugins: {},
        ui: function () {
            return {
                originalElement: this.originalElement,
                element: this.element,
                helper: this.helper,
                position: this.position,
                size: this.size,
                originalSize: this.originalSize,
                originalPosition: this.originalPosition
            }
        }
    }), e.ui.plugin.add("resizable",
		"animate", {
		    stop: function (t) {
		        var i = e(this).data(
					"ui-resizable"),
					s = i.options,
					n = i._proportionallyResizeElements,
					a = n.length && /textarea/i.test(
						n[0].nodeName),
					o = a && e.ui.hasScroll(n[0],
						"left") ? 0 : i.sizeDiff.height,
					r = a ? 0 : i.sizeDiff.width,
					h = {
					    width: i.size.width - r,
					    height: i.size.height - o
					}, l = parseInt(i.element.css(
						"left"), 10) + (i.position.left -
						i.originalPosition.left) || null,
					u = parseInt(i.element.css("top"),
						10) + (i.position.top - i.originalPosition
						.top) || null;
		        i.element.animate(e.extend(h, u &&
					l ? {
					    top: u,
					    left: l
					} : {}), {
					    duration: s.animateDuration,
					    easing: s.animateEasing,
					    step: function () {
					        var s = {
					            width: parseInt(i.element.css(
                                    "width"), 10),
					            height: parseInt(i.element.css(
                                    "height"), 10),
					            top: parseInt(i.element.css(
                                    "top"), 10),
					            left: parseInt(i.element.css(
                                    "left"), 10)
					        };
					        n && n.length && e(n[0]).css({
					            width: s.width,
					            height: s.height
					        }), i._updateCache(s), i._propagate(
                                "resize", t)
					    }
					})
		    }
		}), e.ui.plugin.add("resizable",
		"containment", {
		    start: function () {
		        var i, s, n, a, o, r, h, l = e(
						this).data("ui-resizable"),
					u = l.options,
					c = l.element,
					d = u.containment,
					p = d instanceof e ? d.get(0) :
						/parent/.test(d) ? c.parent().get(
							0) : d;
		        p && (l.containerElement = e(p),
					/document/.test(d) || d ===
					document ? (l.containerOffset = {
					    left: 0,
					    top: 0
					}, l.containerPosition = {
					    left: 0,
					    top: 0
					}, l.parentData = {
					    element: e(document),
					    left: 0,
					    top: 0,
					    width: e(document).width(),
					    height: e(document).height() ||
							document.body.parentNode.scrollHeight
					}) : (i = e(p), s = [], e(["Top",
							"Right", "Left", "Bottom"
					]).each(function (e, n) {
					    s[e] = t(i.css("padding" + n))
					}), l.containerOffset = i.offset(),
						l.containerPosition = i.position(),
						l.containerSize = {
						    height: i.innerHeight() - s[3],
						    width: i.innerWidth() - s[1]
						}, n = l.containerOffset, a = l.containerSize
						.height, o = l.containerSize.width,
						r = e.ui.hasScroll(p, "left") ?
						p.scrollWidth : o, h = e.ui.hasScroll(
							p) ? p.scrollHeight : a, l.parentData = {
							    element: p,
							    left: n.left,
							    top: n.top,
							    width: r,
							    height: h
							}))
		    },
		    resize: function (t) {
		        var i, s, n, a, o = e(this).data(
						"ui-resizable"),
					r = o.options,
					h = o.containerOffset,
					l = o.position,
					u = o._aspectRatio || t.shiftKey,
					c = {
					    top: 0,
					    left: 0
					}, d = o.containerElement;
		        d[0] !== document && /static/.test(
					d.css("position")) && (c = h), l.left <
					(o._helper ? h.left : 0) && (o.size
					.width = o.size.width + (o._helper ?
						o.position.left - h.left : o.position
						.left - c.left), u && (o.size.height =
						o.size.width / o.aspectRatio), o
					.position.left = r.helper ? h.left :
					0), l.top < (o._helper ? h.top :
					0) && (o.size.height = o.size.height +
					(o._helper ? o.position.top - h.top :
						o.position.top), u && (o.size.width =
						o.size.height * o.aspectRatio),
					o.position.top = o._helper ? h.top :
					0), o.offset.left = o.parentData.left +
					o.position.left, o.offset.top = o
					.parentData.top + o.position.top,
					i = Math.abs((o._helper ? o.offset
						.left - c.left : o.offset.left -
						c.left) + o.sizeDiff.width), s =
					Math.abs((o._helper ? o.offset.top -
							c.top : o.offset.top - h.top) +
						o.sizeDiff.height), n = o.containerElement
					.get(0) === o.element.parent().get(
						0), a = /relative|absolute/.test(
						o.containerElement.css(
							"position")), n && a && (i -= o
						.parentData.left), i + o.size.width >=
					o.parentData.width && (o.size.width =
						o.parentData.width - i, u && (o.size
							.height = o.size.width / o.aspectRatio
						)), s + o.size.height >= o.parentData
					.height && (o.size.height = o.parentData
						.height - s, u && (o.size.width =
							o.size.height * o.aspectRatio))
		    },
		    stop: function () {
		        var t = e(this).data(
					"ui-resizable"),
					i = t.options,
					s = t.containerOffset,
					n = t.containerPosition,
					a = t.containerElement,
					o = e(t.helper),
					r = o.offset(),
					h = o.outerWidth() - t.sizeDiff.width,
					l = o.outerHeight() - t.sizeDiff.height;
		        t._helper && !i.animate &&
					/relative/.test(a.css("position")) &&
					e(this).css({
					    left: r.left - n.left - s.left,
					    width: h,
					    height: l
					}), t._helper && !i.animate &&
					/static/.test(a.css("position")) &&
					e(this).css({
					    left: r.left - n.left - s.left,
					    width: h,
					    height: l
					})
		    }
		}), e.ui.plugin.add("resizable",
		"alsoResize", {
		    start: function () {
		        var t = e(this).data(
					"ui-resizable"),
					i = t.options,
					s = function (t) {
					    e(t).each(function () {
					        var t = e(this);
					        t.data(
								"ui-resizable-alsoresize", {
								    width: parseInt(t.width(),
										10),
								    height: parseInt(t.height(),
										10),
								    left: parseInt(t.css("left"),
										10),
								    top: parseInt(t.css("top"),
										10)
								})
					    })
					};
		        "object" != typeof i.alsoResize ||
					i.alsoResize.parentNode ? s(i.alsoResize) :
					i.alsoResize.length ? (i.alsoResize =
						i.alsoResize[0], s(i.alsoResize)
				) : e.each(i.alsoResize, function (
					e) {
				    s(e)
				})
		    },
		    resize: function (t, i) {
		        var s = e(this).data(
					"ui-resizable"),
					n = s.options,
					a = s.originalSize,
					o = s.originalPosition,
					r = {
					    height: s.size.height - a.height ||
							0,
					    width: s.size.width - a.width ||
							0,
					    top: s.position.top - o.top || 0,
					    left: s.position.left - o.left ||
							0
					}, h = function (t, s) {
					    e(t).each(function () {
					        var t = e(this),
								n = e(this).data(
									"ui-resizable-alsoresize"),
								a = {}, o = s && s.length ? s :
									t.parents(i.originalElement[
										0]).length ? ["width",
										"height"
										] : ["width", "height", "top",
									"left"
										];
					        e.each(o, function (e, t) {
					            var i = (n[t] || 0) + (r[t] ||
									0);
					            i && i >= 0 && (a[t] = i ||
									null)
					        }), t.css(a)
					    })
					};
		        "object" != typeof n.alsoResize ||
					n.alsoResize.nodeType ? h(n.alsoResize) :
					e.each(n.alsoResize, function (e,
						t) {
					    h(e, t)
					})
		    },
		    stop: function () {
		        e(this).removeData(
					"resizable-alsoresize")
		    }
		}), e.ui.plugin.add("resizable",
		"ghost", {
		    start: function () {
		        var t = e(this).data(
					"ui-resizable"),
					i = t.options,
					s = t.size;
		        t.ghost = t.originalElement.clone(),
					t.ghost.css({
					    opacity: .25,
					    display: "block",
					    position: "relative",
					    height: s.height,
					    width: s.width,
					    margin: 0,
					    left: 0,
					    top: 0
					}).addClass("ui-resizable-ghost")
					.addClass("string" == typeof i.ghost ?
						i.ghost : ""), t.ghost.appendTo(
						t.helper)
		    },
		    resize: function () {
		        var t = e(this).data(
					"ui-resizable");
		        t.ghost && t.ghost.css({
		            position: "relative",
		            height: t.size.height,
		            width: t.size.width
		        })
		    },
		    stop: function () {
		        var t = e(this).data(
					"ui-resizable");
		        t.ghost && t.helper && t.helper.get(
					0).removeChild(t.ghost.get(0))
		    }
		}), e.ui.plugin.add("resizable",
		"grid", {
		    resize: function () {
		        var t = e(this).data(
					"ui-resizable"),
					i = t.options,
					s = t.size,
					n = t.originalSize,
					a = t.originalPosition,
					o = t.axis,
					r = "number" == typeof i.grid ? [
						i.grid, i.grid
					] : i.grid,
					h = r[0] || 1,
					l = r[1] || 1,
					u = Math.round((s.width - n.width) /
						h) * h,
					c = Math.round((s.height - n.height) /
						l) * l,
					d = n.width + u,
					p = n.height + c,
					f = i.maxWidth && d > i.maxWidth,
					m = i.maxHeight && p > i.maxHeight,
					g = i.minWidth && i.minWidth > d,
					v = i.minHeight && i.minHeight >
						p;
		        i.grid = r, g && (d += h), v && (p +=
					l), f && (d -= h), m && (p -= l),
					/^(se|s|e)$/.test(o) ? (t.size.width =
						d, t.size.height = p) : /^(ne)$/
					.test(o) ? (t.size.width = d, t.size
						.height = p, t.position.top = a.top -
						c) : /^(sw)$/.test(o) ? (t.size.width =
						d, t.size.height = p, t.position
						.left = a.left - u) : (t.size.width =
						d, t.size.height = p, t.position
						.top = a.top - c, t.position.left =
						a.left - u)
		    }
		})
})(jQuery);
(function (e) {
    e.widget("ui.selectable", e.ui.mouse, {
        version: "1.10.3",
        options: {
            appendTo: "body",
            autoRefresh: !0,
            distance: 0,
            filter: "*",
            tolerance: "touch",
            selected: null,
            selecting: null,
            start: null,
            stop: null,
            unselected: null,
            unselecting: null
        },
        _create: function () {
            var t, i = this;
            this.element.addClass(
				"ui-selectable"), this.dragged = !
				1, this.refresh = function () {
				    t = e(i.options.filter, i.element[
						0]), t.addClass("ui-selectee"),
						t.each(function () {
						    var t = e(this),
								i = t.offset();
						    e.data(this,
								"selectable-item", {
								    element: this,
								    $element: t,
								    left: i.left,
								    top: i.top,
								    right: i.left + t.outerWidth(),
								    bottom: i.top + t.outerHeight(),
								    startselected: !1,
								    selected: t.hasClass(
										"ui-selected"),
								    selecting: t.hasClass(
										"ui-selecting"),
								    unselecting: t.hasClass(
										"ui-unselecting")
								})
						})
				}, this.refresh(), this.selectees =
				t.addClass("ui-selectee"), this._mouseInit(),
				this.helper = e(
					"<div class='ui-selectable-helper'></div>"
			)
        },
        _destroy: function () {
            this.selectees.removeClass(
				"ui-selectee").removeData(
				"selectable-item"), this.element.removeClass(
				"ui-selectable ui-selectable-disabled"
			), this._mouseDestroy()
        },
        _mouseStart: function (t) {
            var i = this,
				s = this.options;
            this.opos = [t.pageX, t.pageY],
				this.options.disabled || (this.selectees =
					e(s.filter, this.element[0]),
					this._trigger("start", t), e(s.appendTo)
					.append(this.helper), this.helper
					.css({
					    left: t.pageX,
					    top: t.pageY,
					    width: 0,
					    height: 0
					}), s.autoRefresh && this.refresh(),
					this.selectees.filter(
						".ui-selected").each(function () {
						    var s = e.data(this,
                                "selectable-item");
						    s.startselected = !0, t.metaKey ||
                                t.ctrlKey || (s.$element.removeClass(
                                        "ui-selected"), s.selected = !
                                    1, s.$element.addClass(
                                        "ui-unselecting"), s.unselecting = !
                                    0, i._trigger("unselecting",
                                        t, {
                                            unselecting: s.element
                                        }))
						}), e(t.target).parents().addBack()
					.each(function () {
					    var s, n = e.data(this,
								"selectable-item");
					    return n ? (s = !t.metaKey && !
							t.ctrlKey || !n.$element.hasClass(
								"ui-selected"), n.$element.removeClass(
								s ? "ui-unselecting" :
								"ui-selected").addClass(s ?
								"ui-selecting" :
								"ui-unselecting"), n.unselecting = !
							s, n.selecting = s, n.selected =
							s, s ? i._trigger("selecting",
								t, {
								    selecting: n.element
								}) : i._trigger(
								"unselecting", t, {
								    unselecting: n.element
								}), !1) : undefined
					}))
        },
        _mouseDrag: function (t) {
            if (this.dragged = !0, !this.options
				.disabled) {
                var i, s = this,
					n = this.options,
					a = this.opos[0],
					o = this.opos[1],
					r = t.pageX,
					h = t.pageY;
                return a > r && (i = r, r = a, a =
					i), o > h && (i = h, h = o, o =
					i), this.helper.css({
					    left: a,
					    top: o,
					    width: r - a,
					    height: h - o
					}), this.selectees.each(function () {
					    var i = e.data(this,
                            "selectable-item"),
                            l = !1;
					    i && i.element !== s.element[0] &&
                            ("touch" === n.tolerance ? l = !
                            (i.left > r || a > i.right ||
                                i.top > h || o > i.bottom) :
                            "fit" === n.tolerance && (l =
                                i.left > a && r > i.right &&
                                i.top > o && h > i.bottom), l ?
                            (i.selected && (i.$element.removeClass(
                                    "ui-selected"), i.selected = !
                                1), i.unselecting && (i.$element
                                .removeClass(
                                    "ui-unselecting"), i.unselecting = !
                                1), i.selecting || (i.$element
                                .addClass("ui-selecting"), i
                                .selecting = !0, s._trigger(
                                    "selecting", t, {
                                        selecting: i.element
                                    }))) : (i.selecting && ((t.metaKey ||
                                    t.ctrlKey) && i.startselected ?
                                (i.$element.removeClass(
                                        "ui-selecting"), i.selecting = !
                                    1, i.$element.addClass(
                                        "ui-selected"), i.selected = !
                                    0) : (i.$element.removeClass(
                                        "ui-selecting"), i.selecting = !
                                    1, i.startselected && (i.$element
                                        .addClass("ui-unselecting"),
                                        i.unselecting = !0), s._trigger(
                                        "unselecting", t, {
                                            unselecting: i.element
                                        }))), i.selected && (t.metaKey ||
                                t.ctrlKey || i.startselected ||
                                (i.$element.removeClass(
                                        "ui-selected"), i.selected = !
                                    1, i.$element.addClass(
                                        "ui-unselecting"), i.unselecting = !
                                    0, s._trigger("unselecting",
                                        t, {
                                            unselecting: i.element
                                        })))))
					}), !1
            }
        },
        _mouseStop: function (t) {
            var i = this;
            return this.dragged = !1, e(
				".ui-unselecting", this.element[0]
			).each(function () {
			    var s = e.data(this,
					"selectable-item");
			    s.$element.removeClass(
					"ui-unselecting"), s.unselecting = !
					1, s.startselected = !1, i._trigger(
						"unselected", t, {
						    unselected: s.element
						})
			}), e(".ui-selecting", this.element[
				0]).each(function () {
				    var s = e.data(this,
                        "selectable-item");
				    s.$element.removeClass(
                        "ui-selecting").addClass(
                        "ui-selected"), s.selecting = !
                        1, s.selected = !0, s.startselected = !
                        0, i._trigger("selected", t, {
                            selected: s.element
                        })
				}), this._trigger("stop", t), this
				.helper.remove(), !1
        }
    })
})(jQuery);
(function (t) {
    function e(t, e, i) {
        return t > e && e + i > t
    }

    function i(t) {
        return /left|right/.test(t.css(
			"float")) || /inline|table-cell/.test(
			t.css("display"))
    }
    t.widget("ui.sortable", t.ui.mouse, {
        version: "1.10.3",
        widgetEventPrefix: "sort",
        ready: !1,
        options: {
            appendTo: "parent",
            axis: !1,
            connectWith: !1,
            containment: !1,
            cursor: "auto",
            cursorAt: !1,
            dropOnEmpty: !0,
            forcePlaceholderSize: !1,
            forceHelperSize: !1,
            grid: !1,
            handle: !1,
            helper: "original",
            items: "> *",
            opacity: !1,
            placeholder: !1,
            revert: !1,
            scroll: !0,
            scrollSensitivity: 20,
            scrollSpeed: 20,
            scope: "default",
            tolerance: "intersect",
            zIndex: 1e3,
            activate: null,
            beforeStop: null,
            change: null,
            deactivate: null,
            out: null,
            over: null,
            receive: null,
            remove: null,
            sort: null,
            start: null,
            stop: null,
            update: null
        },
        _create: function () {
            var t = this.options;
            this.containerCache = {}, this.element
				.addClass("ui-sortable"), this.refresh(),
				this.floating = this.items.length ?
				"x" === t.axis || i(this.items[0]
					.item) : !1, this.offset = this.element
				.offset(), this._mouseInit(),
				this.ready = !0
        },
        _destroy: function () {
            this.element.removeClass(
				"ui-sortable ui-sortable-disabled"
			), this._mouseDestroy();
            for (var t = this.items.length - 1; t >=
				0; t--) this.items[t].item.removeData(
				this.widgetName + "-item");
            return this
        },
        _setOption: function (e, i) {
            "disabled" === e ? (this.options[e] =
				i, this.widget().toggleClass(
					"ui-sortable-disabled", !!i)) :
				t.Widget.prototype._setOption.apply(
					this, arguments)
        },
        _mouseCapture: function (e, i) {
            var s = null,
				n = !1,
				a = this;
            return this.reverting ? !1 : this.options
				.disabled || "static" === this.options
				.type ? !1 : (this._refreshItems(
						e), t(e.target).parents().each(
						function () {
						    return t.data(this, a.widgetName +
								"-item") === a ? (s = t(this), !
								1) : undefined
						}), t.data(e.target, a.widgetName +
						"-item") === a && (s = t(e.target)),
					s ? !this.options.handle || i ||
					(t(this.options.handle, s).find(
						"*").addBack().each(function () {
						    this === e.target && (n = !0)
						}), n) ? (this.currentItem = s,
						this._removeCurrentsFromItems(), !
						0) : !1 : !1)
        },
        _mouseStart: function (e, i, s) {
            var n, a, o = this.options;
            if (this.currentContainer = this,
				this.refreshPositions(), this.helper =
				this._createHelper(e), this._cacheHelperProportions(),
				this._cacheMargins(), this.scrollParent =
				this.helper.scrollParent(), this.offset =
				this.currentItem.offset(), this.offset = {
                top: this.offset.top - this.margins
						.top,
                left: this.offset.left - this.margins
						.left
            }, t.extend(this.offset, {
                click: {
                left: e.pageX - this.offset.left,
                top: e.pageY - this.offset.top
            },
                parent: this._getParentOffset(),
                relative: this._getRelativeOffset()
            }), this.helper.css("position",
					"absolute"), this.cssPosition =
				this.helper.css("position"), this
				.originalPosition = this._generatePosition(
					e), this.originalPageX = e.pageX,
				this.originalPageY = e.pageY, o.cursorAt &&
				this._adjustOffsetFromHelper(o.cursorAt),
				this.domPosition = {
                prev: this.currentItem.prev()[0],
                parent: this.currentItem.parent()[
						0]
            }, this.helper[0] !== this.currentItem[
					0] && this.currentItem.hide(),
				this._createPlaceholder(), o.containment &&
				this._setContainment(), o.cursor &&
				"auto" !== o.cursor && (a = this.document
					.find("body"), this.storedCursor =
					a.css("cursor"), a.css("cursor",
						o.cursor), this.storedStylesheet =
					t("<style>*{ cursor: " + o.cursor +
						" !important; }</style>").appendTo(
						a)), o.opacity && (this.helper.css(
						"opacity") && (this._storedOpacity =
						this.helper.css("opacity")),
					this.helper.css("opacity", o.opacity)
				), o.zIndex && (this.helper.css(
						"zIndex") && (this._storedZIndex =
						this.helper.css("zIndex")),
					this.helper.css("zIndex", o.zIndex)
				), this.scrollParent[0] !==
				document && "HTML" !== this.scrollParent[
					0].tagName && (this.overflowOffset =
					this.scrollParent.offset()),
				this._trigger("start", e, this._uiHash()),
				this._preserveHelperProportions ||
				this._cacheHelperProportions(), !
				s)
                for (n = this.containers.length -
					1; n >= 0; n--) this.containers[
					n]._trigger("activate", e, this._uiHash(
					this));
            return t.ui.ddmanager && (t.ui.ddmanager
				.current = this), t.ui.ddmanager && !
				o.dropBehaviour && t.ui.ddmanager
				.prepareOffsets(this, e), this.dragging = !
				0, this.helper.addClass(
					"ui-sortable-helper"), this._mouseDrag(
					e), !0
        },
        _mouseDrag: function (e) {
            var i, s, n, a, o = this.options,
				r = !1;
            for (this.position = this._generatePosition(
					e), this.positionAbs = this._convertPositionTo(
					"absolute"), this.lastPositionAbs ||
				(this.lastPositionAbs = this.positionAbs),
				this.options.scroll && (this.scrollParent[
						0] !== document && "HTML" !==
					this.scrollParent[0].tagName ? (
						this.overflowOffset.top + this.scrollParent[
							0].offsetHeight - e.pageY < o.scrollSensitivity ?
						this.scrollParent[0].scrollTop =
						r = this.scrollParent[0].scrollTop +
						o.scrollSpeed : e.pageY - this.overflowOffset
						.top < o.scrollSensitivity && (
							this.scrollParent[0].scrollTop =
							r = this.scrollParent[0].scrollTop -
							o.scrollSpeed), this.overflowOffset
						.left + this.scrollParent[0].offsetWidth -
						e.pageX < o.scrollSensitivity ?
						this.scrollParent[0].scrollLeft =
						r = this.scrollParent[0].scrollLeft +
						o.scrollSpeed : e.pageX - this.overflowOffset
						.left < o.scrollSensitivity &&
						(this.scrollParent[0].scrollLeft =
							r = this.scrollParent[0].scrollLeft -
							o.scrollSpeed)) : (e.pageY - t(
							document).scrollTop() < o.scrollSensitivity ?
						r = t(document).scrollTop(t(
							document).scrollTop() - o.scrollSpeed) :
						t(window).height() - (e.pageY -
							t(document).scrollTop()) < o.scrollSensitivity &&
						(r = t(document).scrollTop(t(
							document).scrollTop() + o.scrollSpeed)),
						e.pageX - t(document).scrollLeft() <
						o.scrollSensitivity ? r = t(
							document).scrollLeft(t(
							document).scrollLeft() - o.scrollSpeed) :
						t(window).width() - (e.pageX -
							t(document).scrollLeft()) < o.scrollSensitivity &&
						(r = t(document).scrollLeft(t(
							document).scrollLeft() + o.scrollSpeed))
					), r !== !1 && t.ui.ddmanager && !
					o.dropBehaviour && t.ui.ddmanager
					.prepareOffsets(this, e)), this.positionAbs =
				this._convertPositionTo(
					"absolute"), this.options.axis &&
				"y" === this.options.axis || (
					this.helper[0].style.left = this
					.position.left + "px"), this.options
				.axis && "x" === this.options.axis ||
				(this.helper[0].style.top = this.position
					.top + "px"), i = this.items.length -
				1; i >= 0; i--)
                if (s = this.items[i], n = s.item[
						0], a = this._intersectsWithPointer(
						s), a && s.instance === this.currentContainer &&
					n !== this.currentItem[0] &&
					this.placeholder[1 === a ?
						"next" : "prev"]()[0] !== n && !
					t.contains(this.placeholder[0],
						n) && ("semi-dynamic" === this.options
						.type ? !t.contains(this.element[
							0], n) : !0)) {
                    if (this.direction = 1 === a ?
						"down" : "up", "pointer" !==
						this.options.tolerance && !this
						._intersectsWithSides(s)) break;
                    this._rearrange(e, s), this._trigger(
						"change", e, this._uiHash());
                    break
                }
            return this._contactContainers(e),
				t.ui.ddmanager && t.ui.ddmanager.drag(
					this, e), this._trigger("sort",
					e, this._uiHash()), this.lastPositionAbs =
				this.positionAbs, !1
        },
        _mouseStop: function (e, i) {
            if (e) {
                if (t.ui.ddmanager && !this.options
					.dropBehaviour && t.ui.ddmanager
					.drop(this, e), this.options.revert
				) {
                    var s = this,
						n = this.placeholder.offset(),
						a = this.options.axis,
						o = {};
                    a && "x" !== a || (o.left = n.left -
						this.offset.parent.left - this.margins
						.left + (this.offsetParent[0] ===
							document.body ? 0 : this.offsetParent[
								0].scrollLeft)), a && "y" !==
						a || (o.top = n.top - this.offset
							.parent.top - this.margins.top +
							(this.offsetParent[0] ===
								document.body ? 0 : this.offsetParent[
									0].scrollTop)), this.reverting = !
						0, t(this.helper).animate(o,
							parseInt(this.options.revert,
								10) || 500, function () {
								    s._clear(e)
								})
                } else this._clear(e, i);
                return !1
            }
        },
        cancel: function () {
            if (this.dragging) {
                this._mouseUp({
                    target: null
                }), "original" === this.options.helper ?
					this.currentItem.css(this._storedCSS)
					.removeClass(
						"ui-sortable-helper") : this.currentItem
					.show();
                for (var e = this.containers.length -
					1; e >= 0; e--) this.containers[
					e]._trigger("deactivate", null,
					this._uiHash(this)), this.containers[
					e].containerCache.over && (this.containers[
						e]._trigger("out", null, this._uiHash(
						this)), this.containers[e].containerCache
					.over = 0)
            }
            return this.placeholder && (this.placeholder[
					0].parentNode && this.placeholder[
					0].parentNode.removeChild(this.placeholder[
					0]), "original" !== this.options
				.helper && this.helper && this.helper[
					0].parentNode && this.helper.remove(),
				t.extend(this, {
				    helper: null,
				    dragging: !1,
				    reverting: !1,
				    _noFinalSort: null
				}), this.domPosition.prev ? t(
					this.domPosition.prev).after(
					this.currentItem) : t(this.domPosition
					.parent).prepend(this.currentItem)
			), this
        },
        serialize: function (e) {
            var i = this._getItemsAsjQuery(e &&
				e.connected),
				s = [];
            return e = e || {}, t(i).each(
				function () {
				    var i = (t(e.item || this).attr(
						e.attribute || "id") || "").match(
						e.expression ||
						/(.+)[\-=_](.+)/);
				    i && s.push((e.key || i[1] +
						"[]") + "=" + (e.key && e.expression ?
						i[1] : i[2]))
				}), !s.length && e.key && s.push(
				e.key + "="), s.join("&")
        },
        toArray: function (e) {
            var i = this._getItemsAsjQuery(e &&
				e.connected),
				s = [];
            return e = e || {}, i.each(
				function () {
				    s.push(t(e.item || this).attr(e.attribute ||
						"id") || "")
				}), s
        },
        _intersectsWith: function (t) {
            var e = this.positionAbs.left,
				i = e + this.helperProportions.width,
				s = this.positionAbs.top,
				n = s + this.helperProportions.height,
				a = t.left,
				o = a + t.width,
				r = t.top,
				h = r + t.height,
				l = this.offset.click.top,
				c = this.offset.click.left,
				u = "x" === this.options.axis ||
					s + l > r && h > s + l,
				d = "y" === this.options.axis ||
					e + c > a && o > e + c,
				p = u && d;
            return "pointer" === this.options.tolerance ||
				this.options.forcePointerForContainers ||
				"pointer" !== this.options.tolerance &&
				this.helperProportions[this.floating ?
					"width" : "height"] > t[this.floating ?
					"width" : "height"] ? p : e +
				this.helperProportions.width / 2 >
				a && o > i - this.helperProportions
				.width / 2 && s + this.helperProportions
				.height / 2 > r && h > n - this.helperProportions
				.height / 2
        },
        _intersectsWithPointer: function (t) {
            var i = "x" === this.options.axis ||
				e(this.positionAbs.top + this.offset
					.click.top, t.top, t.height),
				s = "y" === this.options.axis ||
					e(this.positionAbs.left + this.offset
						.click.left, t.left, t.width),
				n = i && s,
				a = this._getDragVerticalDirection(),
				o = this._getDragHorizontalDirection();
            return n ? this.floating ? o &&
				"right" === o || "down" === a ? 2 :
				1 : a && ("down" === a ? 2 : 1) : !
				1
        },
        _intersectsWithSides: function (t) {
            var i = e(this.positionAbs.top +
				this.offset.click.top, t.top + t.height /
				2, t.height),
				s = e(this.positionAbs.left +
					this.offset.click.left, t.left +
					t.width / 2, t.width),
				n = this._getDragVerticalDirection(),
				a = this._getDragHorizontalDirection();
            return this.floating && a ?
				"right" === a && s || "left" ===
				a && !s : n && ("down" === n && i ||
					"up" === n && !i)
        },
        _getDragVerticalDirection: function () {
            var t = this.positionAbs.top -
				this.lastPositionAbs.top;
            return 0 !== t && (t > 0 ? "down" :
				"up")
        },
        _getDragHorizontalDirection: function () {
            var t = this.positionAbs.left -
				this.lastPositionAbs.left;
            return 0 !== t && (t > 0 ? "right" :
				"left")
        },
        refresh: function (t) {
            return this._refreshItems(t), this
				.refreshPositions(), this
        },
        _connectWith: function () {
            var t = this.options;
            return t.connectWith.constructor ===
				String ? [t.connectWith] : t.connectWith
        },
        _getItemsAsjQuery: function (e) {
            var i, s, n, a, o = [],
				r = [],
				h = this._connectWith();
            if (h && e)
                for (i = h.length - 1; i >= 0; i--)
                    for (n = t(h[i]), s = n.length -
						1; s >= 0; s--) a = t.data(n[s],
						this.widgetFullName), a && a !==
						this && !a.options.disabled &&
						r.push([t.isFunction(a.options.items) ?
							a.options.items.call(a.element) :
							t(a.options.items, a.element)
							.not(".ui-sortable-helper").not(
								".ui-sortable-placeholder"),
							a
						]);
            for (r.push([t.isFunction(this.options
					.items) ? this.options.items.call(
					this.element, null, {
                options: this.options,
                item: this.currentItem
            }) : t(this.options.items,
					this.element).not(
					".ui-sortable-helper").not(
					".ui-sortable-placeholder"),
				this
            ]), i = r.length - 1; i >= 0; i--)
                r[i][0].each(function () {
                    o.push(this)
                });
            return t(o)
        },
        _removeCurrentsFromItems: function () {
            var e = this.currentItem.find(
				":data(" + this.widgetName +
				"-item)");
            this.items = t.grep(this.items,
				function (t) {
				    for (var i = 0; e.length > i; i++)
				        if (e[i] === t.item[0]) return !
							1;
				    return !0
				})
        },
        _refreshItems: function (e) {
            this.items = [], this.containers = [
				this
            ];
            var i, s, n, a, o, r, h, l, c =
					this.items,
				u = [
					[t.isFunction(this.options.items) ?
						this.options.items.call(this.element[
							0], e, {
							    item: this.currentItem
							}) : t(this.options.items, this
							.element), this
					]
				],
				d = this._connectWith();
            if (d && this.ready)
                for (i = d.length - 1; i >= 0; i--)
                    for (n = t(d[i]), s = n.length -
						1; s >= 0; s--) a = t.data(n[s],
						this.widgetFullName), a && a !==
						this && !a.options.disabled &&
						(u.push([t.isFunction(a.options
							.items) ? a.options.items.call(
							a.element[0], e, {
							    item: this.currentItem
							}) : t(a.options.items, a.element),
						a
						]), this.containers.push(a));
            for (i = u.length - 1; i >= 0; i--)
                for (o = u[i][1], r = u[i][0], s =
					0, l = r.length; l > s; s++) h =
					t(r[s]), h.data(this.widgetName +
						"-item", o), c.push({
						    item: h,
						    instance: o,
						    width: 0,
						    height: 0,
						    left: 0,
						    top: 0
						})
        },
        refreshPositions: function (e) {
            this.offsetParent && this.helper &&
				(this.offset.parent = this._getParentOffset());
            var i, s, n, a;
            for (i = this.items.length - 1; i >=
				0; i--) s = this.items[i], s.instance !==
				this.currentContainer && this.currentContainer &&
				s.item[0] !== this.currentItem[0] ||
				(n = this.options.toleranceElement ?
				t(this.options.toleranceElement,
					s.item) : s.item, e || (s.width =
					n.outerWidth(), s.height = n.outerHeight()
				), a = n.offset(), s.left = a.left,
				s.top = a.top);
            if (this.options.custom && this.options
				.custom.refreshContainers) this.options
				.custom.refreshContainers.call(
					this);
            else
                for (i = this.containers.length -
					1; i >= 0; i--) a = this.containers[
					i].element.offset(), this.containers[
					i].containerCache.left = a.left,
					this.containers[i].containerCache
					.top = a.top, this.containers[i]
					.containerCache.width = this.containers[
						i].element.outerWidth(), this.containers[
						i].containerCache.height = this
					.containers[i].element.outerHeight();
            return this
        },
        _createPlaceholder: function (e) {
            e = e || this;
            var i, s = e.options;
            s.placeholder && s.placeholder.constructor !==
				String || (i = s.placeholder, s.placeholder = {
				    element: function () {
				        var s = e.currentItem[0].nodeName
							.toLowerCase(),
							n = t("<" + s + ">", e.document[
								0]).addClass(i || e.currentItem[
									0].className +
								" ui-sortable-placeholder").removeClass(
								"ui-sortable-helper");
				        return "tr" === s ? e.currentItem
							.children().each(function () {
							    t("<td>&#160;</td>", e.document[
									0]).attr("colspan", t(this)
									.attr("colspan") || 1).appendTo(
									n)
							}) : "img" === s && n.attr(
								"src", e.currentItem.attr(
									"src")), i || n.css(
								"visibility", "hidden"), n
				    },
				    update: function (t, n) {
				        (!i || s.forcePlaceholderSize) &&
							(n.height() || n.height(e.currentItem
								.innerHeight() - parseInt(e.currentItem
									.css("paddingTop") || 0, 10
								) - parseInt(e.currentItem.css(
									"paddingBottom") || 0, 10)),
							n.width() || n.width(e.currentItem
								.innerWidth() - parseInt(e.currentItem
									.css("paddingLeft") || 0,
									10) - parseInt(e.currentItem
									.css("paddingRight") || 0,
									10)))
				    }
				}), e.placeholder = t(s.placeholder
					.element.call(e.element, e.currentItem)
			), e.currentItem.after(e.placeholder),
				s.placeholder.update(e, e.placeholder)
        },
        _contactContainers: function (s) {
            var n, a, o, r, h, l, c, u, d, p,
					f = null,
				m = null;
            for (n = this.containers.length -
				1; n >= 0; n--)
                if (!t.contains(this.currentItem[
					0], this.containers[n].element[
					0]))
                    if (this._intersectsWith(this.containers[
						n].containerCache)) {
                        if (f && t.contains(this.containers[
							n].element[0], f.element[0]))
                            continue;
                        f = this.containers[n], m = n
                    } else this.containers[n].containerCache
						.over && (this.containers[n]._trigger(
								"out", s, this._uiHash(this)),
							this.containers[n].containerCache
							.over = 0);
            if (f)
                if (1 === this.containers.length)
                    this.containers[m].containerCache
						.over || (this.containers[m]._trigger(
								"over", s, this._uiHash(this)
							), this.containers[m].containerCache
							.over = 1);
                else {
                    for (o = 1e4, r = null, p = f.floating ||
						i(this.currentItem), h = p ?
						"left" : "top", l = p ? "width" :
						"height", c = this.positionAbs[
							h] + this.offset.click[h], a =
						this.items.length - 1; a >= 0; a--
					) t.contains(this.containers[m].element[
						0], this.items[a].item[0]) &&
						this.items[a].item[0] !== this.currentItem[
							0] && (!p || e(this.positionAbs
							.top + this.offset.click.top,
							this.items[a].top, this.items[
								a].height)) && (u = this.items[
								a].item.offset()[h], d = !1,
							Math.abs(u - c) > Math.abs(u +
								this.items[a][l] - c) && (d = !
								0, u += this.items[a][l]), o >
							Math.abs(u - c) && (o = Math.abs(
									u - c), r = this.items[a],
								this.direction = d ? "up" :
								"down"));
                    if (!r && !this.options.dropOnEmpty)
                        return;
                    if (this.currentContainer ===
						this.containers[m]) return;
                    r ? this._rearrange(s, r, null, !
						0) : this._rearrange(s, null,
						this.containers[m].element, !0),
						this._trigger("change", s, this
							._uiHash()), this.containers[m]
						._trigger("change", s, this._uiHash(
							this)), this.currentContainer =
						this.containers[m], this.options
						.placeholder.update(this.currentContainer,
							this.placeholder), this.containers[
							m]._trigger("over", s, this._uiHash(
							this)), this.containers[m].containerCache
						.over = 1
                }
        },
        _createHelper: function (e) {
            var i = this.options,
				s = t.isFunction(i.helper) ? t(i.helper
					.apply(this.element[0], [e, this
						.currentItem
					])) : "clone" === i.helper ?
					this.currentItem.clone() : this.currentItem;
            return s.parents("body").length ||
				t("parent" !== i.appendTo ? i.appendTo :
					this.currentItem[0].parentNode)[
					0].appendChild(s[0]), s[0] ===
				this.currentItem[0] && (this._storedCSS = {
				    width: this.currentItem[0].style
						.width,
				    height: this.currentItem[0].style
						.height,
				    position: this.currentItem.css(
						"position"),
				    top: this.currentItem.css("top"),
				    left: this.currentItem.css(
						"left")
				}), (!s[0].style.width || i.forceHelperSize) &&
				s.width(this.currentItem.width()), (!
					s[0].style.height || i.forceHelperSize
			) && s.height(this.currentItem.height()),
				s
        },
        _adjustOffsetFromHelper: function (
			e) {
            "string" == typeof e && (e = e.split(
				" ")), t.isArray(e) && (e = {
				    left: +e[0],
				    top: +e[1] || 0
				}), "left" in e && (this.offset.click
				.left = e.left + this.margins.left
			), "right" in e && (this.offset.click
				.left = this.helperProportions.width -
				e.right + this.margins.left),
				"top" in e && (this.offset.click.top =
					e.top + this.margins.top),
				"bottom" in e && (this.offset.click
					.top = this.helperProportions.height -
					e.bottom + this.margins.top)
        },
        _getParentOffset: function () {
            this.offsetParent = this.helper.offsetParent();
            var e = this.offsetParent.offset();
            return "absolute" === this.cssPosition &&
				this.scrollParent[0] !== document &&
				t.contains(this.scrollParent[0],
					this.offsetParent[0]) && (e.left +=
					this.scrollParent.scrollLeft(),
					e.top += this.scrollParent.scrollTop()
			), (this.offsetParent[0] ===
				document.body || this.offsetParent[
					0].tagName && "html" === this.offsetParent[
					0].tagName.toLowerCase() && t.ui
				.ie) && (e = {
				    top: 0,
				    left: 0
				}), {
				    top: e.top + (parseInt(this.offsetParent
                            .css("borderTopWidth"), 10) ||
                        0),
				    left: e.left + (parseInt(this.offsetParent
                            .css("borderLeftWidth"), 10) ||
                        0)
				}
        },
        _getRelativeOffset: function () {
            if ("relative" === this.cssPosition) {
                var t = this.currentItem.position();
                return {
                    top: t.top - (parseInt(this.helper
						.css("top"), 10) || 0) + this.scrollParent
						.scrollTop(),
                    left: t.left - (parseInt(this.helper
						.css("left"), 10) || 0) + this.scrollParent
						.scrollLeft()
                }
            }
            return {
                top: 0,
                left: 0
            }
        },
        _cacheMargins: function () {
            this.margins = {
                left: parseInt(this.currentItem.css(
					"marginLeft"), 10) || 0,
                top: parseInt(this.currentItem.css(
					"marginTop"), 10) || 0
            }
        },
        _cacheHelperProportions: function () {
            this.helperProportions = {
                width: this.helper.outerWidth(),
                height: this.helper.outerHeight()
            }
        },
        _setContainment: function () {
            var e, i, s, n = this.options;
            "parent" === n.containment && (n.containment =
				this.helper[0].parentNode), (
				"document" === n.containment ||
				"window" === n.containment) && (
				this.containment = [0 - this.offset
					.relative.left - this.offset.parent
					.left, 0 - this.offset.relative.top -
					this.offset.parent.top, t(
						"document" === n.containment ?
					    document : window).width() -
					this.helperProportions.width -
					this.margins.left, (t("document" ===
							n.containment ? document :
							window).height() || document.body
						.parentNode.scrollHeight) -
					this.helperProportions.height -
					this.margins.top
				]), /^(document|window|parent)$/.test(
				n.containment) || (e = t(n.containment)[
					0], i = t(n.containment).offset(),
				s = "hidden" !== t(e).css(
					"overflow"), this.containment = [
					i.left + (parseInt(t(e).css(
						"borderLeftWidth"), 10) || 0) +
					(parseInt(t(e).css("paddingLeft"),
						10) || 0) - this.margins.left,
					i.top + (parseInt(t(e).css(
						"borderTopWidth"), 10) || 0) +
					(parseInt(t(e).css("paddingTop"),
						10) || 0) - this.margins.top, i
					.left + (s ? Math.max(e.scrollWidth,
						e.offsetWidth) : e.offsetWidth) -
					(parseInt(t(e).css(
						"borderLeftWidth"), 10) || 0) -
					(parseInt(t(e).css(
						"paddingRight"), 10) || 0) -
					this.helperProportions.width -
					this.margins.left, i.top + (s ?
						Math.max(e.scrollHeight, e.offsetHeight) :
						e.offsetHeight) - (parseInt(t(e)
							.css("borderTopWidth"), 10) ||
						0) - (parseInt(t(e).css(
						"paddingBottom"), 10) || 0) -
					this.helperProportions.height -
					this.margins.top
					])
        },
        _convertPositionTo: function (e, i) {
            i || (i = this.position);
            var s = "absolute" === e ? 1 : -1,
				n = "absolute" !== this.cssPosition ||
					this.scrollParent[0] !==
					document && t.contains(this.scrollParent[
						0], this.offsetParent[0]) ?
					this.scrollParent : this.offsetParent,
				a = /(html|body)/i.test(n[0].tagName);
            return {
                top: i.top + this.offset.relative
					.top * s + this.offset.parent.top *
					s - ("fixed" === this.cssPosition ? -
						this.scrollParent.scrollTop() :
						a ? 0 : n.scrollTop()) * s,
                left: i.left + this.offset.relative
					.left * s + this.offset.parent.left *
					s - ("fixed" === this.cssPosition ? -
						this.scrollParent.scrollLeft() :
						a ? 0 : n.scrollLeft()) * s
            }
        },
        _generatePosition: function (e) {
            var i, s, n = this.options,
				a = e.pageX,
				o = e.pageY,
				r = "absolute" !== this.cssPosition ||
					this.scrollParent[0] !==
					document && t.contains(this.scrollParent[
						0], this.offsetParent[0]) ?
					this.scrollParent : this.offsetParent,
				h = /(html|body)/i.test(r[0].tagName);
            return "relative" !== this.cssPosition ||
				this.scrollParent[0] !== document &&
				this.scrollParent[0] !== this.offsetParent[
					0] || (this.offset.relative =
					this._getRelativeOffset()), this
				.originalPosition && (this.containment &&
					(e.pageX - this.offset.click.left <
						this.containment[0] && (a =
							this.containment[0] + this.offset
							.click.left), e.pageY - this.offset
						.click.top < this.containment[1] &&
						(o = this.containment[1] + this
							.offset.click.top), e.pageX -
						this.offset.click.left > this.containment[
							2] && (a = this.containment[2] +
							this.offset.click.left), e.pageY -
						this.offset.click.top > this.containment[
							3] && (o = this.containment[3] +
							this.offset.click.top)), n.grid &&
					(i = this.originalPageY + Math.round(
							(o - this.originalPageY) / n.grid[
								1]) * n.grid[1], o = this.containment ?
						i - this.offset.click.top >=
						this.containment[1] && i - this
						.offset.click.top <= this.containment[
							3] ? i : i - this.offset.click
						.top >= this.containment[1] ? i -
						n.grid[1] : i + n.grid[1] : i,
						s = this.originalPageX + Math.round(
							(a - this.originalPageX) / n.grid[
								0]) * n.grid[0], a = this.containment ?
						s - this.offset.click.left >=
						this.containment[0] && s - this
						.offset.click.left <= this.containment[
							2] ? s : s - this.offset.click
						.left >= this.containment[0] ?
						s - n.grid[0] : s + n.grid[0] :
						s)), {
						    top: o - this.offset.click.top -
                                this.offset.relative.top - this
                                .offset.parent.top + ("fixed" ===
                                    this.cssPosition ? -this.scrollParent
                                    .scrollTop() : h ? 0 : r.scrollTop()
                            ),
						    left: a - this.offset.click.left -
                                this.offset.relative.left -
                                this.offset.parent.left + (
                                    "fixed" === this.cssPosition ? -
                                    this.scrollParent.scrollLeft() :
                                    h ? 0 : r.scrollLeft())
						}
        },
        _rearrange: function (t, e, i, s) {
            i ? i[0].appendChild(this.placeholder[
				0]) : e.item[0].parentNode.insertBefore(
				this.placeholder[0], "down" ===
				this.direction ? e.item[0] : e.item[
					0].nextSibling), this.counter =
				this.counter ? ++this.counter : 1;
            var n = this.counter;
            this._delay(function () {
                n === this.counter && this.refreshPositions(!
					s)
            })
        },
        _clear: function (t, e) {
            this.reverting = !1;
            var i, s = [];
            if (!this._noFinalSort && this.currentItem
				.parent().length && this.placeholder
				.before(this.currentItem), this._noFinalSort =
				null, this.helper[0] === this.currentItem[
					0]) {
                for (i in this._storedCSS) ("auto" ===
					this._storedCSS[i] || "static" ===
					this._storedCSS[i]) && (this._storedCSS[
					i] = "");
                this.currentItem.css(this._storedCSS)
					.removeClass(
						"ui-sortable-helper")
            } else this.currentItem.show();
            for (this.fromOutside && !e && s.push(
					function (t) {
						this._trigger("receive", t,
							this._uiHash(this.fromOutside)
						)
            }), !this.fromOutside && this.domPosition
				.prev === this.currentItem.prev()
				.not(".ui-sortable-helper")[0] &&
				this.domPosition.parent === this.currentItem
				.parent()[0] || e || s.push(
					function (t) {
						this._trigger("update", t, this
							._uiHash())
            }), this !== this.currentContainer &&
				(e || (s.push(function (t) {
						this._trigger("remove", t,
							this._uiHash())
            }), s.push(function (t) {
						return function (e) {
							t._trigger("receive", e,
								this._uiHash(this))
            }
            }.call(this, this.currentContainer)),
					s.push(function (t) {
						return function (e) {
							t._trigger("update", e, this
								._uiHash(this))
            }
            }.call(this, this.currentContainer))
				)), i = this.containers.length -
				1; i >= 0; i--) e || s.push(
				function (t) {
				    return function (e) {
				        t._trigger("deactivate", e,
							this._uiHash(this))
				    }
				}.call(this, this.containers[i])),
				this.containers[i].containerCache
				.over && (s.push(function (t) {
				    return function (e) {
				        t._trigger("out", e, this._uiHash(
                            this))
				    }
				}.call(this, this.containers[i])),
					this.containers[i].containerCache
					.over = 0);
            if (this.storedCursor && (this.document
					.find("body").css("cursor", this
						.storedCursor), this.storedStylesheet
					.remove()), this._storedOpacity &&
				this.helper.css("opacity", this._storedOpacity),
				this._storedZIndex && this.helper
				.css("zIndex", "auto" === this._storedZIndex ?
					"" : this._storedZIndex), this.dragging = !
				1, this.cancelHelperRemoval) {
                if (!e) {
                    for (this._trigger("beforeStop",
							t, this._uiHash()), i = 0; s.length >
						i; i++) s[i].call(this, t);
                    this._trigger("stop", t, this._uiHash())
                }
                return this.fromOutside = !1, !1
            }
            if (e || this._trigger(
					"beforeStop", t, this._uiHash()),
				this.placeholder[0].parentNode.removeChild(
					this.placeholder[0]), this.helper[
					0] !== this.currentItem[0] &&
				this.helper.remove(), this.helper =
				null, !e) {
                for (i = 0; s.length > i; i++) s[
					i].call(this, t);
                this._trigger("stop", t, this._uiHash())
            }
            return this.fromOutside = !1, !0
        },
        _trigger: function () {
            t.Widget.prototype._trigger.apply(
				this, arguments) === !1 && this.cancel()
        },
        _uiHash: function (e) {
            var i = e || this;
            return {
                helper: i.helper,
                placeholder: i.placeholder || t([]),
                position: i.position,
                originalPosition: i.originalPosition,
                offset: i.positionAbs,
                item: i.currentItem,
                sender: e ? e.element : null
            }
        }
    })
})(jQuery);
(function (t) {
    var e = 0,
		i = {}, s = {};
    i.height = i.paddingTop = i.paddingBottom =
		i.borderTopWidth = i.borderBottomWidth =
		"hide", s.height = s.paddingTop = s.paddingBottom =
		s.borderTopWidth = s.borderBottomWidth =
		"show", t.widget("ui.accordion", {
		    version: "1.10.3",
		    options: {
		        active: 0,
		        animate: {},
		        collapsible: !1,
		        event: "click",
		        header: "> li > :first-child,> :not(li):even",
		        heightStyle: "auto",
		        icons: {
		            activeHeader: "ui-icon-triangle-1-s",
		            header: "ui-icon-triangle-1-e"
		        },
		        activate: null,
		        beforeActivate: null
		    },
		    _create: function () {
		        var e = this.options;
		        this.prevShow = this.prevHide = t(),
					this.element.addClass(
						"ui-accordion ui-widget ui-helper-reset"
				).attr("role", "tablist"), e.collapsible ||
					e.active !== !1 && null != e.active ||
					(e.active = 0), this._processPanels(),
					0 > e.active && (e.active +=
						this.headers.length), this._refresh()
		    },
		    _getCreateEventData: function () {
		        return {
		            header: this.active,
		            panel: this.active.length ? this
						.active.next() : t(),
		            content: this.active.length ?
						this.active.next() : t()
		        }
		    },
		    _createIcons: function () {
		        var e = this.options.icons;
		        e && (t("<span>").addClass(
						"ui-accordion-header-icon ui-icon " +
						e.header).prependTo(this.headers),
					this.active.children(
						".ui-accordion-header-icon").removeClass(
						e.header).addClass(e.activeHeader),
					this.headers.addClass(
						"ui-accordion-icons"))
		    },
		    _destroyIcons: function () {
		        this.headers.removeClass(
					"ui-accordion-icons").children(
					".ui-accordion-header-icon").remove()
		    },
		    _destroy: function () {
		        var t;
		        this.element.removeClass(
					"ui-accordion ui-widget ui-helper-reset"
				).removeAttr("role"), this.headers
					.removeClass(
						"ui-accordion-header ui-accordion-header-active ui-helper-reset ui-state-default ui-corner-all ui-state-active ui-state-disabled ui-corner-top"
				).removeAttr("role").removeAttr(
					"aria-selected").removeAttr(
					"aria-controls").removeAttr(
					"tabIndex").each(function () {
					    /^ui-accordion/.test(this.id) &&
                            this.removeAttribute("id")
					}), this._destroyIcons(), t =
					this.headers.next().css(
						"display", "").removeAttr(
						"role").removeAttr(
						"aria-expanded").removeAttr(
						"aria-hidden").removeAttr(
						"aria-labelledby").removeClass(
						"ui-helper-reset ui-widget-content ui-corner-bottom ui-accordion-content ui-accordion-content-active ui-state-disabled"
				).each(function () {
				    /^ui-accordion/.test(this.id) &&
						this.removeAttribute("id")
				}), "content" !== this.options.heightStyle &&
					t.css("height", "")
		    },
		    _setOption: function (t, e) {
		        return "active" === t ? (this._activate(
					e), undefined) : ("event" === t &&
					(this.options.event && this._off(
						this.headers, this.options.event
					), this._setupEvents(e)), this._super(
						t, e), "collapsible" !== t || e ||
					this.options.active !== !1 ||
					this._activate(0), "icons" === t &&
					(this._destroyIcons(), e && this
						._createIcons()), "disabled" ===
					t && this.headers.add(this.headers
						.next()).toggleClass(
						"ui-state-disabled", !!e),
					undefined)
		    },
		    _keydown: function (e) {
		        if (!e.altKey && !e.ctrlKey) {
		            var i = t.ui.keyCode,
						s = this.headers.length,
						n = this.headers.index(e.target),
						a = !1;
		            switch (e.keyCode) {
		                case i.RIGHT:
		                case i.DOWN:
		                    a = this.headers[(n + 1) % s];
		                    break;
		                case i.LEFT:
		                case i.UP:
		                    a = this.headers[(n - 1 + s) %
                                s];
		                    break;
		                case i.SPACE:
		                case i.ENTER:
		                    this._eventHandler(e);
		                    break;
		                case i.HOME:
		                    a = this.headers[0];
		                    break;
		                case i.END:
		                    a = this.headers[s - 1]
		            }
		            a && (t(e.target).attr(
						"tabIndex", -1), t(a).attr(
						"tabIndex", 0), a.focus(), e.preventDefault())
		        }
		    },
		    _panelKeyDown: function (e) {
		        e.keyCode === t.ui.keyCode.UP &&
					e.ctrlKey && t(e.currentTarget).prev()
					.focus()
		    },
		    refresh: function () {
		        var e = this.options;
		        this._processPanels(), e.active === !
					1 && e.collapsible === !0 || !
					this.headers.length ? (e.active = !
						1, this.active = t()) : e.active === !
					1 ? this._activate(0) : this.active
					.length && !t.contains(this.element[
						0], this.active[0]) ? this.headers
					.length === this.headers.find(
						".ui-state-disabled").length ?
					(e.active = !1, this.active = t()) :
					this._activate(Math.max(0, e.active -
						1)) : e.active = this.headers.index(
						this.active), this._destroyIcons(),
					this._refresh()
		    },
		    _processPanels: function () {
		        this.headers = this.element.find(
					this.options.header).addClass(
					"ui-accordion-header ui-helper-reset ui-state-default ui-corner-all"
				), this.headers.next().addClass(
					"ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom"
				).filter(
					":not(.ui-accordion-content-active)"
				).hide()
		    },
		    _refresh: function () {
		        var i, s = this.options,
					n = s.heightStyle,
					a = this.element.parent(),
					o = this.accordionId =
						"ui-accordion-" + (this.element
							.attr("id") || ++e);
		        this.active = this._findActive(s.active)
					.addClass(
						"ui-accordion-header-active ui-state-active ui-corner-top"
				).removeClass("ui-corner-all"),
					this.active.next().addClass(
						"ui-accordion-content-active").show(),
					this.headers.attr("role", "tab")
					.each(function (e) {
					    var i = t(this),
							s = i.attr("id"),
							n = i.next(),
							a = n.attr("id");
					    s || (s = o + "-header-" + e,
							i.attr("id", s)), a || (a = o +
							"-panel-" + e, n.attr("id", a)
						), i.attr("aria-controls", a),
							n.attr("aria-labelledby", s)
					}).next().attr("role",
						"tabpanel"), this.headers.not(
						this.active).attr({
						    "aria-selected": "false",
						    tabIndex: -1
						}).next().attr({
						    "aria-expanded": "false",
						    "aria-hidden": "true"
						}).hide(), this.active.length ?
					this.active.attr({
					    "aria-selected": "true",
					    tabIndex: 0
					}).next().attr({
					    "aria-expanded": "true",
					    "aria-hidden": "false"
					}) : this.headers.eq(0).attr(
						"tabIndex", 0), this._createIcons(),
					this._setupEvents(s.event),
					"fill" === n ? (i = a.height(),
						this.element.siblings(
							":visible").each(function () {
							    var e = t(this),
                                    s = e.css("position");
							    "absolute" !== s && "fixed" !==
                                    s && (i -= e.outerHeight(!0))
							}), this.headers.each(function () {
							    i -= t(this).outerHeight(!0)
							}), this.headers.next().each(
							function () {
							    t(this).height(Math.max(0, i -
									t(this).innerHeight() + t(
										this).height()))
							}).css("overflow", "auto")) :
					"auto" === n && (i = 0, this.headers
						.next().each(function () {
						    i = Math.max(i, t(this).css(
								"height", "").height())
						}).height(i))
		    },
		    _activate: function (e) {
		        var i = this._findActive(e)[0];
		        i !== this.active[0] && (i = i ||
					this.active[0], this._eventHandler({
					    target: i,
					    currentTarget: i,
					    preventDefault: t.noop
					}))
		    },
		    _findActive: function (e) {
		        return "number" == typeof e ?
					this.headers.eq(e) : t()
		    },
		    _setupEvents: function (e) {
		        var i = {
		            keydown: "_keydown"
		        };
		        e && t.each(e.split(" "),
					function (t, e) {
					    i[e] = "_eventHandler"
					}), this._off(this.headers.add(
					this.headers.next())), this._on(
					this.headers, i), this._on(this.headers
					.next(), {
					    keydown: "_panelKeyDown"
					}), this._hoverable(this.headers),
					this._focusable(this.headers)
		    },
		    _eventHandler: function (e) {
		        var i = this.options,
					s = this.active,
					n = t(e.currentTarget),
					a = n[0] === s[0],
					o = a && i.collapsible,
					r = o ? t() : n.next(),
					h = s.next(),
					l = {
					    oldHeader: s,
					    oldPanel: h,
					    newHeader: o ? t() : n,
					    newPanel: r
					};
		        e.preventDefault(), a && !i.collapsible ||
					this._trigger("beforeActivate",
						e, l) === !1 || (i.active = o ? !
						1 : this.headers.index(n), this
						.active = a ? t() : n, this._toggle(
							l), s.removeClass(
							"ui-accordion-header-active ui-state-active"
						), i.icons && s.children(
							".ui-accordion-header-icon").removeClass(
							i.icons.activeHeader).addClass(
							i.icons.header), a || (n.removeClass(
							"ui-corner-all").addClass(
							"ui-accordion-header-active ui-state-active ui-corner-top"
						), i.icons && n.children(
							".ui-accordion-header-icon").removeClass(
							i.icons.header).addClass(i.icons
							.activeHeader), n.next().addClass(
							"ui-accordion-content-active"
						)))
		    },
		    _toggle: function (e) {
		        var i = e.newPanel,
					s = this.prevShow.length ? this.prevShow :
						e.oldPanel;
		        this.prevShow.add(this.prevHide).stop(!
					0, !0), this.prevShow = i, this.prevHide =
					s, this.options.animate ? this._animate(
						i, s, e) : (s.hide(), i.show(),
						this._toggleComplete(e)), s.attr({
						    "aria-expanded": "false",
						    "aria-hidden": "true"
						}), s.prev().attr(
						"aria-selected", "false"), i.length &&
					s.length ? s.prev().attr(
						"tabIndex", -1) : i.length &&
					this.headers.filter(function () {
					    return 0 === t(this).attr(
							"tabIndex")
					}).attr("tabIndex", -1), i.attr({
					    "aria-expanded": "true",
					    "aria-hidden": "false"
					}).prev().attr({
					    "aria-selected": "true",
					    tabIndex: 0
					})
		    },
		    _animate: function (t, e, n) {
		        var a, o, r, h = this,
					l = 0,
					c = t.length && (!e.length || t.index() <
						e.index()),
					u = this.options.animate || {},
						d = c && u.down || u,
					p = function () {
					    h._toggleComplete(n)
					};
		        return "number" == typeof d && (r =
					d), "string" == typeof d && (o =
					d), o = o || d.easing || u.easing,
					r = r || d.duration || u.duration,
					e.length ? t.length ? (a = t.show()
						.outerHeight(), e.animate(i, {
						    duration: r,
						    easing: o,
						    step: function (t, e) {
						        e.now = Math.round(t)
						    }
						}), t.hide().animate(s, {
						    duration: r,
						    easing: o,
						    complete: p,
						    step: function (t, i) {
						        i.now = Math.round(t),
									"height" !== i.prop ? l +=
									i.now : "content" !== h.options
									.heightStyle && (i.now =
										Math.round(a - e.outerHeight() -
											l), l = 0)
						    }
						}), undefined) : e.animate(i, r,
						o, p) : t.animate(s, r, o, p)
		    },
		    _toggleComplete: function (t) {
		        var e = t.oldPanel;
		        e.removeClass(
					"ui-accordion-content-active").prev()
					.removeClass("ui-corner-top").addClass(
						"ui-corner-all"), e.length && (
						e.parent()[0].className = e.parent()[
							0].className), this._trigger(
						"activate", null, t)
		    }
		})
})(jQuery);
(function (t) {
    var e = 5;
    t.widget("ui.slider", t.ui.mouse, {
        version: "1.10.3",
        widgetEventPrefix: "slide",
        options: {
            animate: !1,
            distance: 0,
            max: 100,
            min: 0,
            orientation: "horizontal",
            range: !1,
            step: 1,
            value: 0,
            values: null,
            change: null,
            slide: null,
            start: null,
            stop: null
        },
        _create: function () {
            this._keySliding = !1, this._mouseSliding = !
				1, this._animateOff = !0, this._handleIndex =
				null, this._detectOrientation(),
				this._mouseInit(), this.element.addClass(
					"ui-slider ui-slider-" + this.orientation +
					" ui-widget" +
					" ui-widget-content" +
					" ui-corner-all"), this._refresh(),
				this._setOption("disabled", this.options
					.disabled), this._animateOff = !
				1
        },
        _refresh: function () {
            this._createRange(), this._createHandles(),
				this._setupEvents(), this._refreshValue()
        },
        _createHandles: function () {
            var e, i, s = this.options,
				n = this.element.find(
					".ui-slider-handle").addClass(
					"ui-state-default ui-corner-all"
				),
				a =
					"<a class='ui-slider-handle ui-state-default ui-corner-all' href='#'></a>",
				o = [];
            for (i = s.values && s.values.length ||
				1, n.length > i && (n.slice(i).remove(),
					n = n.slice(0, i)), e = n.length; i >
				e; e++) o.push(a);
            this.handles = n.add(t(o.join(""))
				.appendTo(this.element)), this.handle =
				this.handles.eq(0), this.handles.each(
					function (e) {
					    t(this).data(
							"ui-slider-handle-index", e)
					})
        },
        _createRange: function () {
            var e = this.options,
				i = "";
            e.range ? (e.range === !0 && (e.values ?
					e.values.length && 2 !== e.values
					.length ? e.values = [e.values[0],
						e.values[0]
					] : t.isArray(e.values) && (e.values =
						e.values.slice(0)) : e.values = [
						this._valueMin(), this._valueMin()
						]), this.range && this.range.length ?
				this.range.removeClass(
					"ui-slider-range-min ui-slider-range-max"
				).css({
				    left: "",
				    bottom: ""
				}) : (this.range = t(
						"<div></div>").appendTo(this.element),
					i =
					"ui-slider-range ui-widget-header ui-corner-all"
				), this.range.addClass(i + ("min" ===
					e.range || "max" === e.range ?
					" ui-slider-range-" + e.range :
					""))) : this.range = t([])
        },
        _setupEvents: function () {
            var t = this.handles.add(this.range)
				.filter("a");
            this._off(t), this._on(t, this._handleEvents),
				this._hoverable(t), this._focusable(
					t)
        },
        _destroy: function () {
            this.handles.remove(), this.range.remove(),
				this.element.removeClass(
					"ui-slider ui-slider-horizontal ui-slider-vertical ui-widget ui-widget-content ui-corner-all"
			), this._mouseDestroy()
        },
        _mouseCapture: function (e) {
            var i, s, n, a, o, r, h, l, u =
					this,
				c = this.options;
            return c.disabled ? !1 : (this.elementSize = {
                width: this.element.outerWidth(),
                height: this.element.outerHeight()
            }, this.elementOffset = this.element
				.offset(), i = {
				    x: e.pageX,
				    y: e.pageY
				}, s = this._normValueFromMouse(i),
				n = this._valueMax() - this._valueMin() +
				1, this.handles.each(function (e) {
				    var i = Math.abs(s - u.values(e));
				    (n > i || n === i && (e === u._lastChangedValue ||
						u.values(e) === c.min)) && (n =
						i, a = t(this), o = e)
				}), r = this._start(e, o), r === !
				1 ? !1 : (this._mouseSliding = !0,
					this._handleIndex = o, a.addClass(
						"ui-state-active").focus(), h =
					a.offset(), l = !t(e.target).parents()
					.addBack().is(
						".ui-slider-handle"), this._clickOffset =
					l ? {
					    left: 0,
					    top: 0
					} : {
					    left: e.pageX - h.left - a.width() /
							2,
					    top: e.pageY - h.top - a.height() /
							2 - (parseInt(a.css(
								"borderTopWidth"), 10) || 0) -
							(parseInt(a.css(
								"borderBottomWidth"), 10) ||
							0) + (parseInt(a.css(
								"marginTop"), 10) || 0)
					}, this.handles.hasClass(
						"ui-state-hover") || this._slide(
						e, o, s), this._animateOff = !0, !
					0))
        },
        _mouseStart: function () {
            return !0
        },
        _mouseDrag: function (t) {
            var e = {
                x: t.pageX,
                y: t.pageY
            }, i = this._normValueFromMouse(e);
            return this._slide(t, this._handleIndex,
				i), !1
        },
        _mouseStop: function (t) {
            return this.handles.removeClass(
				"ui-state-active"), this._mouseSliding = !
				1, this._stop(t, this._handleIndex),
				this._change(t, this._handleIndex),
				this._handleIndex = null, this._clickOffset =
				null, this._animateOff = !1, !1
        },
        _detectOrientation: function () {
            this.orientation = "vertical" ===
				this.options.orientation ?
				"vertical" : "horizontal"
        },
        _normValueFromMouse: function (t) {
            var e, i, s, n, a;
            return "horizontal" === this.orientation ?
				(e = this.elementSize.width, i =
				t.x - this.elementOffset.left - (
					this._clickOffset ? this._clickOffset
					.left : 0)) : (e = this.elementSize
				.height, i = t.y - this.elementOffset
				.top - (this._clickOffset ? this._clickOffset
					.top : 0)), s = i / e, s > 1 &&
				(s = 1), 0 > s && (s = 0),
				"vertical" === this.orientation &&
				(s = 1 - s), n = this._valueMax() -
				this._valueMin(), a = this._valueMin() +
				s * n, this._trimAlignValue(a)
        },
        _start: function (t, e) {
            var i = {
                handle: this.handles[e],
                value: this.value()
            };
            return this.options.values && this
				.options.values.length && (i.value =
					this.values(e), i.values = this.values()
			), this._trigger("start", t, i)
        },
        _slide: function (t, e, i) {
            var s, n, a;
            this.options.values && this.options
				.values.length ? (s = this.values(
						e ? 0 : 1), 2 === this.options.values
					.length && this.options.range === !
					0 && (0 === e && i > s || 1 ===
						e && s > i) && (i = s), i !==
					this.values(e) && (n = this.values(),
						n[e] = i, a = this._trigger(
							"slide", t, {
							    handle: this.handles[e],
							    value: i,
							    values: n
							}), s = this.values(e ? 0 : 1),
						a !== !1 && this.values(e, i, !
							0))) : i !== this.value() && (
					a = this._trigger("slide", t, {
					    handle: this.handles[e],
					    value: i
					}), a !== !1 && this.value(i))
        },
        _stop: function (t, e) {
            var i = {
                handle: this.handles[e],
                value: this.value()
            };
            this.options.values && this.options
				.values.length && (i.value = this
					.values(e), i.values = this.values()
			), this._trigger("stop", t, i)
        },
        _change: function (t, e) {
            if (!this._keySliding && !this._mouseSliding) {
                var i = {
                    handle: this.handles[e],
                    value: this.value()
                };
                this.options.values && this.options
					.values.length && (i.value =
						this.values(e), i.values = this
						.values()), this._lastChangedValue =
					e, this._trigger("change", t, i)
            }
        },
        value: function (t) {
            return arguments.length ? (this.options
				.value = this._trimAlignValue(t),
				this._refreshValue(), this._change(
					null, 0), undefined) : this._value()
        },
        values: function (e, i) {
            var s, n, a;
            if (arguments.length > 1) return this
				.options.values[e] = this._trimAlignValue(
					i), this._refreshValue(), this._change(
					null, e), undefined;
            if (!arguments.length) return this
				._values();
            if (!t.isArray(arguments[0]))
                return this.options.values &&
					this.options.values.length ?
					this._values(e) : this.value();
            for (s = this.options.values, n =
				arguments[0], a = 0; s.length > a; a +=
				1) s[a] = this._trimAlignValue(n[
				a]), this._change(null, a);
            this._refreshValue()
        },
        _setOption: function (e, i) {
            var s, n = 0;
            switch ("range" === e && this.options
				.range === !0 && ("min" === i ? (
					this.options.value = this._values(
						0), this.options.values = null
				) : "max" === i && (this.options
					.value = this._values(this.options
						.values.length - 1), this.options
					.values = null)), t.isArray(this
					.options.values) && (n = this.options
					.values.length), t.Widget.prototype
				._setOption.apply(this, arguments),
				e) {
                case "orientation":
                    this._detectOrientation(), this.element
                        .removeClass(
                            "ui-slider-horizontal ui-slider-vertical"
                    ).addClass("ui-slider-" + this.orientation),
                        this._refreshValue();
                    break;
                case "value":
                    this._animateOff = !0, this._refreshValue(),
                        this._change(null, 0), this._animateOff = !
                        1;
                    break;
                case "values":
                    for (this._animateOff = !0, this._refreshValue(),
                        s = 0; n > s; s += 1) this._change(
                        null, s);
                    this._animateOff = !1;
                    break;
                case "min":
                case "max":
                    this._animateOff = !0, this._refreshValue(),
                        this._animateOff = !1;
                    break;
                case "range":
                    this._animateOff = !0, this._refresh(),
                        this._animateOff = !1
            }
        },
        _value: function () {
            var t = this.options.value;
            return t = this._trimAlignValue(t)
        },
        _values: function (t) {
            var e, i, s;
            if (arguments.length) return e =
				this.options.values[t], e = this._trimAlignValue(
					e);
            if (this.options.values && this.options
				.values.length) {
                for (i = this.options.values.slice(),
					s = 0; i.length > s; s += 1) i[s] =
					this._trimAlignValue(i[s]);
                return i
            }
            return []
        },
        _trimAlignValue: function (t) {
            if (this._valueMin() >= t) return this
				._valueMin();
            if (t >= this._valueMax()) return this
				._valueMax();
            var e = this.options.step > 0 ?
				this.options.step : 1,
				i = (t - this._valueMin()) % e,
				s = t - i;
            return 2 * Math.abs(i) >= e && (s +=
				i > 0 ? e : -e), parseFloat(s.toFixed(
				5))
        },
        _valueMin: function () {
            return this.options.min
        },
        _valueMax: function () {
            return this.options.max
        },
        _refreshValue: function () {
            var e, i, s, n, a, o = this.options
					.range,
				r = this.options,
				h = this,
				l = this._animateOff ? !1 : r.animate,
				u = {};
            this.options.values && this.options
				.values.length ? this.handles.each(
					function (s) {
					    i = 100 * ((h.values(s) - h._valueMin()) /
							(h._valueMax() - h._valueMin())
						), u["horizontal" === h.orientation ?
							"left" : "bottom"] = i + "%",
							t(this).stop(1, 1)[l ?
								"animate" : "css"](u, r.animate),
							h.options.range === !0 && (
								"horizontal" === h.orientation ?
								(0 === s && h.range.stop(1, 1)[
									l ? "animate" : "css"]({
									    left: i + "%"
									}, r.animate), 1 === s && h.range[
									l ? "animate" : "css"]({
									    width: i - e + "%"
									}, {
									    queue: !1,
									    duration: r.animate
									})) : (0 === s && h.range.stop(
										1, 1)[l ? "animate" : "css"]
									({
									    bottom: i + "%"
									}, r.animate), 1 === s && h.range[
										l ? "animate" : "css"]({
										    height: i - e + "%"
										}, {
										    queue: !1,
										    duration: r.animate
										}))), e = i
					}) : (s = this.value(), n = this
					._valueMin(), a = this._valueMax(),
					i = a !== n ? 100 * ((s - n) / (
						a - n)) : 0, u["horizontal" ===
						this.orientation ? "left" :
						"bottom"] = i + "%", this.handle
					.stop(1, 1)[l ? "animate" :
						"css"](u, r.animate), "min" ===
					o && "horizontal" === this.orientation &&
					this.range.stop(1, 1)[l ?
						"animate" : "css"]({
						    width: i + "%"
						}, r.animate), "max" === o &&
					"horizontal" === this.orientation &&
					this.range[l ? "animate" : "css"]
					({
					    width: 100 - i + "%"
					}, {
					    queue: !1,
					    duration: r.animate
					}), "min" === o && "vertical" ===
					this.orientation && this.range.stop(
						1, 1)[l ? "animate" : "css"]({
						    height: i + "%"
						}, r.animate), "max" === o &&
					"vertical" === this.orientation &&
					this.range[l ? "animate" : "css"]
					({
					    height: 100 - i + "%"
					}, {
					    queue: !1,
					    duration: r.animate
					}))
        },
        _handleEvents: {
            keydown: function (i) {
                var s, n, a, o, r = t(i.target).data(
						"ui-slider-handle-index");
                switch (i.keyCode) {
                    case t.ui.keyCode.HOME:
                    case t.ui.keyCode.END:
                    case t.ui.keyCode.PAGE_UP:
                    case t.ui.keyCode.PAGE_DOWN:
                    case t.ui.keyCode.UP:
                    case t.ui.keyCode.RIGHT:
                    case t.ui.keyCode.DOWN:
                    case t.ui.keyCode.LEFT:
                        if (i.preventDefault(), !this._keySliding &&
                            (this._keySliding = !0, t(i.target)
                                .addClass("ui-state-active"),
                                s = this._start(i, r), s === !
                                1)) return
                }
                switch (o = this.options.step, n =
					a = this.options.values && this.options
					.values.length ? this.values(r) :
					this.value(), i.keyCode) {
                    case t.ui.keyCode.HOME:
                        a = this._valueMin();
                        break;
                    case t.ui.keyCode.END:
                        a = this._valueMax();
                        break;
                    case t.ui.keyCode.PAGE_UP:
                        a = this._trimAlignValue(n + (
                            this._valueMax() - this._valueMin()
                        ) / e);
                        break;
                    case t.ui.keyCode.PAGE_DOWN:
                        a = this._trimAlignValue(n - (
                            this._valueMax() - this._valueMin()
                        ) / e);
                        break;
                    case t.ui.keyCode.UP:
                    case t.ui.keyCode.RIGHT:
                        if (n === this._valueMax())
                            return;
                        a = this._trimAlignValue(n + o);
                        break;
                    case t.ui.keyCode.DOWN:
                    case t.ui.keyCode.LEFT:
                        if (n === this._valueMin())
                            return;
                        a = this._trimAlignValue(n - o)
                }
                this._slide(i, r, a)
            },
            click: function (t) {
                t.preventDefault()
            },
            keyup: function (e) {
                var i = t(e.target).data(
					"ui-slider-handle-index");
                this._keySliding && (this._keySliding = !
					1, this._stop(e, i), this._change(
						e, i), t(e.target).removeClass(
						"ui-state-active"))
            }
        }
    })
})(jQuery);
(function (t, e) {
    function i() {
        return ++n
    }

    function s(t) {
        return t.hash.length > 1 &&
			decodeURIComponent(t.href.replace(a,
				"")) === decodeURIComponent(
				location.href.replace(a, ""))
    }
    var n = 0,
		a = /#.*$/;
    t.widget("ui.tabs", {
        version: "1.10.3",
        delay: 300,
        options: {
            active: null,
            collapsible: !1,
            event: "click",
            heightStyle: "content",
            hide: null,
            show: null,
            activate: null,
            beforeActivate: null,
            beforeLoad: null,
            load: null
        },
        _create: function () {
            var e = this,
				i = this.options;
            this.running = !1, this.element.addClass(
				"ui-tabs ui-widget ui-widget-content ui-corner-all"
			).toggleClass(
				"ui-tabs-collapsible", i.collapsible
			).delegate(".ui-tabs-nav > li",
				"mousedown" + this.eventNamespace,
				function (e) {
				    t(this).is(".ui-state-disabled") &&
						e.preventDefault()
				}).delegate(".ui-tabs-anchor",
				"focus" + this.eventNamespace,
				function () {
				    t(this).closest("li").is(
						".ui-state-disabled") && this.blur()
				}), this._processTabs(), i.active =
				this._initialActive(), t.isArray(
					i.disabled) && (i.disabled = t.unique(
					i.disabled.concat(t.map(this.tabs
						.filter(".ui-state-disabled"),
						function (t) {
						    return e.tabs.index(t)
						}))).sort()), this.active =
				this.options.active !== !1 &&
				this.anchors.length ? this._findActive(
					i.active) : t(), this._refresh(),
				this.active.length && this.load(i
					.active)
        },
        _initialActive: function () {
            var i = this.options.active,
				s = this.options.collapsible,
				n = location.hash.substring(1);
            return null === i && (n && this.tabs
				.each(function (s, a) {
				    return t(a).attr(
						"aria-controls") === n ? (i =
						s, !1) : e
				}), null === i && (i = this.tabs.index(
					this.tabs.filter(
						".ui-tabs-active"))), (null ===
					i || -1 === i) && (i = this.tabs
					.length ? 0 : !1)), i !== !1 &&
				(i = this.tabs.index(this.tabs.eq(
				i)), -1 === i && (i = s ? !1 : 0)), !
				s && i === !1 && this.anchors.length &&
				(i = 0), i
        },
        _getCreateEventData: function () {
            return {
                tab: this.active,
                panel: this.active.length ? this._getPanelForTab(
					this.active) : t()
            }
        },
        _tabKeydown: function (i) {
            var s = t(this.document[0].activeElement)
				.closest("li"),
				n = this.tabs.index(s),
				a = !0;
            if (!this._handlePageNav(i)) {
                switch (i.keyCode) {
                    case t.ui.keyCode.RIGHT:
                    case t.ui.keyCode.DOWN:
                        n++;
                        break;
                    case t.ui.keyCode.UP:
                    case t.ui.keyCode.LEFT:
                        a = !1, n--;
                        break;
                    case t.ui.keyCode.END:
                        n = this.anchors.length - 1;
                        break;
                    case t.ui.keyCode.HOME:
                        n = 0;
                        break;
                    case t.ui.keyCode.SPACE:
                        return i.preventDefault(),
                            clearTimeout(this.activating),
                            this._activate(n), e;
                    case t.ui.keyCode.ENTER:
                        return i.preventDefault(),
                            clearTimeout(this.activating),
                            this._activate(n === this.options
                                .active ? !1 : n), e;
                    default:
                        return
                }
                i.preventDefault(), clearTimeout(
					this.activating), n = this._focusNextTab(
					n, a), i.ctrlKey || (s.attr(
						"aria-selected", "false"), this
					.tabs.eq(n).attr("aria-selected",
						"true"), this.activating = this
					._delay(function () {
					    this.option("active", n)
					}, this.delay))
            }
        },
        _panelKeydown: function (e) {
            this._handlePageNav(e) || e.ctrlKey &&
				e.keyCode === t.ui.keyCode.UP &&
				(e.preventDefault(), this.active.focus())
        },
        _handlePageNav: function (i) {
            return i.altKey && i.keyCode === t
				.ui.keyCode.PAGE_UP ? (this._activate(
					this._focusNextTab(this.options
						.active - 1, !1)), !0) : i.altKey &&
				i.keyCode === t.ui.keyCode.PAGE_DOWN ?
				(this._activate(this._focusNextTab(
					this.options.active + 1, !0)), !
				0) : e
        },
        _findNextTab: function (e, i) {
            function s() {
                return e > n && (e = 0), 0 > e &&
					(e = n), e
            }
            for (var n = this.tabs.length - 1; -
				1 !== t.inArray(s(), this.options
					.disabled) ;) e = i ? e + 1 : e -
				1;
            return e
        },
        _focusNextTab: function (t, e) {
            return t = this._findNextTab(t, e),
				this.tabs.eq(t).focus(), t
        },
        _setOption: function (t, i) {
            return "active" === t ? (this._activate(
				i), e) : "disabled" === t ? (this
				._setupDisabled(i), e) : (this._super(
					t, i), "collapsible" === t && (
					this.element.toggleClass(
						"ui-tabs-collapsible", i), i ||
					this.options.active !== !1 ||
					this._activate(0)), "event" ===
				t && this._setupEvents(i),
				"heightStyle" === t && this._setupHeightStyle(
					i), e)
        },
        _tabId: function (t) {
            return t.attr("aria-controls") ||
				"ui-tabs-" + i()
        },
        _sanitizeSelector: function (t) {
            return t ? t.replace(
				/[!"$%&'()*+,.\/:;<=>?@\[\]\^`{|}~]/g,
				"\\$&") : ""
        },
        refresh: function () {
            var e = this.options,
				i = this.tablist.children(
					":has(a[href])");
            e.disabled = t.map(i.filter(
				".ui-state-disabled"), function (
				t) {
				    return i.index(t)
				}), this._processTabs(), e.active !== !
				1 && this.anchors.length ? this.active
				.length && !t.contains(this.tablist[
					0], this.active[0]) ? this.tabs.length ===
				e.disabled.length ? (e.active = !
					1, this.active = t()) : this._activate(
					this._findNextTab(Math.max(0, e.active -
						1), !1)) : e.active = this.tabs
				.index(this.active) : (e.active = !
					1, this.active = t()), this._refresh()
        },
        _refresh: function () {
            this._setupDisabled(this.options.disabled),
				this._setupEvents(this.options.event),
				this._setupHeightStyle(this.options
					.heightStyle), this.tabs.not(
					this.active).attr({
					    "aria-selected": "false",
					    tabIndex: -1
					}), this.panels.not(this._getPanelForTab(
					this.active)).hide().attr({
					    "aria-expanded": "false",
					    "aria-hidden": "true"
					}), this.active.length ? (this.active
					.addClass(
						"ui-tabs-active ui-state-active"
					).attr({
					    "aria-selected": "true",
					    tabIndex: 0
					}), this._getPanelForTab(this.active)
					.show().attr({
					    "aria-expanded": "true",
					    "aria-hidden": "false"
					})) : this.tabs.eq(0).attr(
					"tabIndex", 0)
        },
        _processTabs: function () {
            var e = this;
            this.tablist = this._getList().addClass(
				"ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all"
			).attr("role", "tablist"), this.tabs =
				this.tablist.find(
					"> li:has(a[href])").addClass(
					"ui-state-default ui-corner-top"
			).attr({
			    role: "tab",
			    tabIndex: -1
			}), this.anchors = this.tabs.map(
				function () {
				    return t("a", this)[0]
				}).addClass("ui-tabs-anchor").attr({
				    role: "presentation",
				    tabIndex: -1
				}), this.panels = t(), this.anchors
				.each(function (i, n) {
				    var a, o, r, h = t(n).uniqueId()
							.attr("id"),
						l = t(n).closest("li"),
						u = l.attr("aria-controls");
				    s(n) ? (a = n.hash, o = e.element
						.find(e._sanitizeSelector(a))) :
						(r = e._tabId(l), a = "#" + r,
						o = e.element.find(a), o.length ||
						(o = e._createPanel(r), o.insertAfter(
							e.panels[i - 1] || e.tablist
						)), o.attr("aria-live",
							"polite")), o.length && (e.panels =
						e.panels.add(o)), u && l.data(
						"ui-tabs-aria-controls", u), l
						.attr({
						    "aria-controls": a.substring(
								1),
						    "aria-labelledby": h
						}), o.attr("aria-labelledby",
							h)
				}), this.panels.addClass(
					"ui-tabs-panel ui-widget-content ui-corner-bottom"
			).attr("role", "tabpanel")
        },
        _getList: function () {
            return this.element.find("ol,ul").eq(
				0)
        },
        _createPanel: function (e) {
            return t("<div>").attr("id", e).addClass(
				"ui-tabs-panel ui-widget-content ui-corner-bottom"
			).data("ui-tabs-destroy", !0)
        },
        _setupDisabled: function (e) {
            t.isArray(e) && (e.length ? e.length ===
				this.anchors.length && (e = !0) :
				e = !1);
            for (var i, s = 0; i = this.tabs[s]; s++)
                e === !0 || -1 !== t.inArray(s, e) ?
					t(i).addClass(
						"ui-state-disabled").attr(
						"aria-disabled", "true") : t(i)
					.removeClass("ui-state-disabled")
					.removeAttr("aria-disabled");
            this.options.disabled = e
        },
        _setupEvents: function (e) {
            var i = {
                click: function (t) {
                    t.preventDefault()
                }
            };
            e && t.each(e.split(" "), function (
				t, e) {
                i[e] = "_eventHandler"
            }), this._off(this.anchors.add(
				this.tabs).add(this.panels)),
				this._on(this.anchors, i), this._on(
					this.tabs, {
					    keydown: "_tabKeydown"
					}), this._on(this.panels, {
					    keydown: "_panelKeydown"
					}), this._focusable(this.tabs),
				this._hoverable(this.tabs)
        },
        _setupHeightStyle: function (e) {
            var i, s = this.element.parent();
            "fill" === e ? (i = s.height(), i -=
				this.element.outerHeight() - this
				.element.height(), this.element.siblings(
					":visible").each(function () {
					    var e = t(this),
                            s = e.css("position");
					    "absolute" !== s && "fixed" !==
                            s && (i -= e.outerHeight(!0))
					}), this.element.children().not(
					this.panels).each(function () {
					    i -= t(this).outerHeight(!0)
					}), this.panels.each(function () {
					    t(this).height(Math.max(0, i -
                            t(this).innerHeight() + t(
                                this).height()))
					}).css("overflow", "auto")) :
				"auto" === e && (i = 0, this.panels
					.each(function () {
					    i = Math.max(i, t(this).height(
							"").height())
					}).height(i))
        },
        _eventHandler: function (e) {
            var i = this.options,
				s = this.active,
				n = t(e.currentTarget),
				a = n.closest("li"),
				o = a[0] === s[0],
				r = o && i.collapsible,
				h = r ? t() : this._getPanelForTab(
					a),
				l = s.length ? this._getPanelForTab(
					s) : t(),
				u = {
				    oldTab: s,
				    oldPanel: l,
				    newTab: r ? t() : a,
				    newPanel: h
				};
            e.preventDefault(), a.hasClass(
				"ui-state-disabled") || a.hasClass(
				"ui-tabs-loading") || this.running ||
				o && !i.collapsible || this._trigger(
					"beforeActivate", e, u) === !1 ||
				(i.active = r ? !1 : this.tabs.index(
					a), this.active = o ? t() : a,
				this.xhr && this.xhr.abort(), l.length ||
				h.length || t.error(
					"jQuery UI Tabs: Mismatching fragment identifier."
				), h.length && this.load(this.tabs
					.index(a), e), this._toggle(e, u)
			)
        },
        _toggle: function (e, i) {
            function s() {
                a.running = !1, a._trigger(
					"activate", e, i)
            }

            function n() {
                i.newTab.closest("li").addClass(
					"ui-tabs-active ui-state-active"
				), o.length && a.options.show ? a
					._show(o, a.options.show, s) : (
						o.show(), s())
            }
            var a = this,
				o = i.newPanel,
				r = i.oldPanel;
            this.running = !0, r.length &&
				this.options.hide ? this._hide(r,
					this.options.hide, function () {
					    i.oldTab.closest("li").removeClass(
							"ui-tabs-active ui-state-active"
						), n()
					}) : (i.oldTab.closest("li").removeClass(
					"ui-tabs-active ui-state-active"
				), r.hide(), n()), r.attr({
				    "aria-expanded": "false",
				    "aria-hidden": "true"
				}), i.oldTab.attr("aria-selected",
					"false"), o.length && r.length ?
				i.oldTab.attr("tabIndex", -1) : o
				.length && this.tabs.filter(
					function () {
					    return 0 === t(this).attr(
							"tabIndex")
					}).attr("tabIndex", -1), o.attr({
					    "aria-expanded": "true",
					    "aria-hidden": "false"
					}), i.newTab.attr({
					    "aria-selected": "true",
					    tabIndex: 0
					})
        },
        _activate: function (e) {
            var i, s = this._findActive(e);
            s[0] !== this.active[0] && (s.length ||
				(s = this.active), i = s.find(
					".ui-tabs-anchor")[0], this._eventHandler({
					    target: i,
					    currentTarget: i,
					    preventDefault: t.noop
					}))
        },
        _findActive: function (e) {
            return e === !1 ? t() : this.tabs.eq(
				e)
        },
        _getIndex: function (t) {
            return "string" == typeof t && (t =
				this.anchors.index(this.anchors.filter(
					"[href$='" + t + "']"))), t
        },
        _destroy: function () {
            this.xhr && this.xhr.abort(), this
				.element.removeClass(
					"ui-tabs ui-widget ui-widget-content ui-corner-all ui-tabs-collapsible"
			), this.tablist.removeClass(
				"ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all"
			).removeAttr("role"), this.anchors
				.removeClass("ui-tabs-anchor").removeAttr(
					"role").removeAttr("tabIndex").removeUniqueId(),
				this.tabs.add(this.panels).each(
					function () {
					    t.data(this, "ui-tabs-destroy") ?
							t(this).remove() : t(this).removeClass(
								"ui-state-default ui-state-active ui-state-disabled ui-corner-top ui-corner-bottom ui-widget-content ui-tabs-active ui-tabs-panel"
						).removeAttr("tabIndex").removeAttr(
							"aria-live").removeAttr(
							"aria-busy").removeAttr(
							"aria-selected").removeAttr(
							"aria-labelledby").removeAttr(
							"aria-hidden").removeAttr(
							"aria-expanded").removeAttr(
							"role")
					}), this.tabs.each(function () {
					    var e = t(this),
                            i = e.data(
                                "ui-tabs-aria-controls");
					    i ? e.attr("aria-controls", i).removeData(
                            "ui-tabs-aria-controls") : e.removeAttr(
                            "aria-controls")
					}), this.panels.show(), "content" !==
				this.options.heightStyle && this.panels
				.css("height", "")
        },
        enable: function (i) {
            var s = this.options.disabled;
            s !== !1 && (i === e ? s = !1 : (i =
				this._getIndex(i), s = t.isArray(
					s) ? t.map(s, function (t) {
					    return t !== i ? t : null
					}) : t.map(this.tabs, function (
					t, e) {
					    return e !== i ? e : null
					})), this._setupDisabled(s))
        },
        disable: function (i) {
            var s = this.options.disabled;
            if (s !== !0) {
                if (i === e) s = !0;
                else {
                    if (i = this._getIndex(i), -1 !==
						t.inArray(i, s)) return;
                    s = t.isArray(s) ? t.merge([i],
						s).sort() : [i]
                }
                this._setupDisabled(s)
            }
        },
        load: function (e, i) {
            e = this._getIndex(e);
            var n = this,
				a = this.tabs.eq(e),
				o = a.find(".ui-tabs-anchor"),
				r = this._getPanelForTab(a),
				h = {
				    tab: a,
				    panel: r
				};
            s(o[0]) || (this.xhr = t.ajax(this
					._ajaxSettings(o, i, h)), this.xhr &&
				"canceled" !== this.xhr.statusText &&
				(a.addClass("ui-tabs-loading"), r
					.attr("aria-busy", "true"), this
					.xhr.success(function (t) {
					    setTimeout(function () {
					        r.html(t), n._trigger("load",
								i, h)
					    }, 1)
					}).complete(function (t, e) {
					    setTimeout(function () {
					        "abort" === e && n.panels.stop(!
								1, !0), a.removeClass(
								"ui-tabs-loading"), r.removeAttr(
								"aria-busy"), t === n.xhr &&
								delete n.xhr
					    }, 1)
					})))
        },
        _ajaxSettings: function (e, i, s) {
            var n = this;
            return {
                url: e.attr("href"),
                beforeSend: function (e, a) {
                    return n._trigger("beforeLoad",
						i, t.extend({
						    jqXHR: e,
						    ajaxSettings: a
						}, s))
                }
            }
        },
        _getPanelForTab: function (e) {
            var i = t(e).attr("aria-controls");
            return this.element.find(this._sanitizeSelector(
				"#" + i))
        }
    })
})(jQuery);
(function (t, e) {
    var i = "ui-effects-";
    t.effects = {
        effect: {}
    },
	function (t, e) {
	    function i(t, e, i) {
	        var s = u[e.type] || {};
	        return null == t ? i || !e.def ?
				null : e.def : (t = s.floor ? ~~t :
					parseFloat(t), isNaN(t) ? e.def :
					s.mod ? (t + s.mod) % s.mod : 0 >
					t ? 0 : t > s.max ? s.max : t)
	    }

	    function s(i) {
	        var s = l(),
				n = s._rgba = [];
	        return i = i.toLowerCase(), f(h,
				function (t, a) {
				    var o, r = a.re.exec(i),
						h = r && a.parse(r),
						l = a.space || "rgba";
				    return h ? (o = s[l](h), s[c[l].cache] =
						o[c[l].cache], n = s._rgba = o._rgba, !
						1) : e
				}), n.length ? ("0,0,0,0" === n.join() &&
				t.extend(n, a.transparent), s) : a[
				i]
	    }

	    function n(t, e, i) {
	        return i = (i + 1) % 1, 1 > 6 * i ?
				t + 6 * (e - t) * i : 1 > 2 * i ?
	            e : 2 > 3 * i ? t + 6 * (e - t) *
				(2 / 3 - i) : t
	    }
	    var a, o =
				"backgroundColor borderBottomColor borderLeftColor borderRightColor borderTopColor color columnRuleColor outlineColor textDecorationColor textEmphasisColor",
			r = /^([\-+])=\s*(\d+\.?\d*)/,
			h = [{
			    re: /rgba?\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*,\s*(\d{1,3})\s*(?:,\s*(\d?(?:\.\d+)?)\s*)?\)/,
			    parse: function (t) {
			        return [t[1], t[2], t[3], t[4]]
			    }
			}, {
			    re: /rgba?\(\s*(\d+(?:\.\d+)?)\%\s*,\s*(\d+(?:\.\d+)?)\%\s*,\s*(\d+(?:\.\d+)?)\%\s*(?:,\s*(\d?(?:\.\d+)?)\s*)?\)/,
			    parse: function (t) {
			        return [2.55 * t[1], 2.55 * t[2],
						2.55 * t[3], t[4]]
			    }
			}, {
			    re: /#([a-f0-9]{2})([a-f0-9]{2})([a-f0-9]{2})/,
			    parse: function (t) {
			        return [parseInt(t[1], 16),
						parseInt(t[2], 16), parseInt(t[
							3], 16)]
			    }
			}, {
			    re: /#([a-f0-9])([a-f0-9])([a-f0-9])/,
			    parse: function (t) {
			        return [parseInt(t[1] + t[1], 16),
						parseInt(t[2] + t[2], 16),
						parseInt(t[3] + t[3], 16)]
			    }
			}, {
			    re: /hsla?\(\s*(\d+(?:\.\d+)?)\s*,\s*(\d+(?:\.\d+)?)\%\s*,\s*(\d+(?:\.\d+)?)\%\s*(?:,\s*(\d?(?:\.\d+)?)\s*)?\)/,
			    space: "hsla",
			    parse: function (t) {
			        return [t[1], t[2] / 100, t[3] /
						100, t[4]]
			    }
			}],
			l = t.Color = function (e, i, s, n) {
			    return new t.Color.fn.parse(e, i,
					s, n)
			}, c = {
			    rgba: {
			        props: {
			            red: {
			                idx: 0,
			                type: "byte"
			            },
			            green: {
			                idx: 1,
			                type: "byte"
			            },
			            blue: {
			                idx: 2,
			                type: "byte"
			            }
			        }
			    },
			    hsla: {
			        props: {
			            hue: {
			                idx: 0,
			                type: "degrees"
			            },
			            saturation: {
			                idx: 1,
			                type: "percent"
			            },
			            lightness: {
			                idx: 2,
			                type: "percent"
			            }
			        }
			    }
			}, u = {
			    "byte": {
			        floor: !0,
			        max: 255
			    },
			    percent: {
			        max: 1
			    },
			    degrees: {
			        mod: 360,
			        floor: !0
			    }
			}, d = l.support = {}, p = t("<p>")[
				0],
			f = t.each;
	    p.style.cssText =
			"background-color:rgba(1,1,1,.5)",
			d.rgba = p.style.backgroundColor.indexOf(
				"rgba") > -1, f(c, function (t, e) {
				    e.cache = "_" + t, e.props.alpha = {
				        idx: 3,
				        type: "percent",
				        def: 1
				    }
				}), l.fn = t.extend(l.prototype, {
				    parse: function (n, o, r, h) {
				        if (n === e) return this._rgba = [
                            null, null, null, null
				        ], this;
				        (n.jquery || n.nodeType) && (n =
                            t(n).css(o), o = e);
				        var u = this,
                            d = t.type(n),
                            p = this._rgba = [];
				        return o !== e && (n = [n, o, r,
                            h
				        ], d = "array"), "string" === d ?
                            this.parse(s(n) || a._default) :
                            "array" === d ? (f(c.rgba.props,
                                function (t, e) {
                                    p[e.idx] = i(n[e.idx], e)
                                }), this) : "object" === d ? (
                                n instanceof l ? f(c, function (
                                    t, e) {
                                    n[e.cache] && (u[e.cache] =
                                        n[e.cache].slice())
                                }) : f(c, function (e, s) {
                                    var a = s.cache;
                                    f(s.props, function (t, e) {
                                        if (!u[a] && s.to) {
                                            if ("alpha" === t || null ==
                                                n[t]) return;
                                            u[a] = s.to(u._rgba)
                                        }
                                        u[a][e.idx] = i(n[t], e, !
                                            0)
                                    }), u[a] && 0 > t.inArray(
                                        null, u[a].slice(0, 3)) &&
                                        (u[a][3] = 1, s.from && (u._rgba =
                                        s.from(u[a])))
                                }), this) : e
				    },
				    is: function (t) {
				        var i = l(t),
                            s = !0,
                            n = this;
				        return f(c, function (t, a) {
				            var o, r = i[a.cache];
				            return r && (o = n[a.cache] ||
                                a.to && a.to(n._rgba) || [],
                                f(a.props, function (t, i) {
                                    return null != r[i.idx] ? s =
                                        r[i.idx] === o[i.idx] : e
                                })), s
				        }), s
				    },
				    _space: function () {
				        var t = [],
                            e = this;
				        return f(c, function (i, s) {
				            e[s.cache] && t.push(i)
				        }), t.pop()
				    },
				    transition: function (t, e) {
				        var s = l(t),
                            n = s._space(),
                            a = c[n],
                            o = 0 === this.alpha() ? l(
                                "transparent") : this,
                            r = o[a.cache] || a.to(o._rgba),
                            h = r.slice();
				        return s = s[a.cache], f(a.props,
                            function (t, n) {
                                var a = n.idx,
                                    o = r[a],
                                    l = s[a],
                                    c = u[n.type] || {};
                                null !== l && (null === o ? h[
                                    a] = l : (c.mod && (l - o >
                                    c.mod / 2 ? o += c.mod : o -
                                    l > c.mod / 2 && (o -= c.mod)
                                ), h[a] = i((l - o) * e + o,
                                    n)))
                            }), this[n](h)
				    },
				    blend: function (e) {
				        if (1 === this._rgba[3]) return this;
				        var i = this._rgba.slice(),
                            s = i.pop(),
                            n = l(e)._rgba;
				        return l(t.map(i, function (t, e) {
				            return (1 - s) * n[e] + s * t
				        }))
				    },
				    toRgbaString: function () {
				        var e = "rgba(",
                            i = t.map(this._rgba, function (
                                t, e) {
                                return null == t ? e > 2 ? 1 :
                                    0 : t
                            });
				        return 1 === i[3] && (i.pop(), e =
                            "rgb("), e + i.join() + ")"
				    },
				    toHslaString: function () {
				        var e = "hsla(",
                            i = t.map(this.hsla(), function (
                                t, e) {
                                return null == t && (t = e >
                                    2 ? 1 : 0), e && 3 > e && (t =
                                    Math.round(100 * t) + "%"),
                                    t
                            });
				        return 1 === i[3] && (i.pop(), e =
                            "hsl("), e + i.join() + ")"
				    },
				    toHexString: function (e) {
				        var i = this._rgba.slice(),
                            s = i.pop();
				        return e && i.push(~~(255 * s)),
                            "#" + t.map(i, function (t) {
                                return t = (t || 0).toString(
                                    16), 1 === t.length ? "0" +
                                    t : t
                            }).join("")
				    },
				    toString: function () {
				        return 0 === this._rgba[3] ?
                            "transparent" : this.toRgbaString()
				    }
				}), l.fn.parse.prototype = l.fn, c.hsla
			.to = function (t) {
			    if (null == t[0] || null == t[1] ||
					null == t[2]) return [null, null,
					null, t[3]];
			    var e, i, s = t[0] / 255,
					n = t[1] / 255,
					a = t[2] / 255,
					o = t[3],
					r = Math.max(s, n, a),
					h = Math.min(s, n, a),
					l = r - h,
					c = r + h,
					u = .5 * c;
			    return e = h === r ? 0 : s === r ?
					60 * (n - a) / l + 360 : n === r ?
					60 * (a - s) / l + 120 : 60 * (s -
						n) / l + 240, i = 0 === l ? 0 :
					.5 >= u ? l / c : l / (2 - c), [
						Math.round(e) % 360, i, u, null ==
						o ? 1 : o
					]
			}, c.hsla.from = function (t) {
			    if (null == t[0] || null == t[1] ||
                    null == t[2]) return [null, null,
                    null, t[3]];
			    var e = t[0] / 360,
                    i = t[1],
                    s = t[2],
                    a = t[3],
                    o = .5 >= s ? s * (1 + i) : s + i -
                        s * i,
                    r = 2 * s - o;
			    return [Math.round(255 * n(r, o, e +
                    1 / 3)), Math.round(255 * n(r, o,
                    e)), Math.round(255 * n(r, o, e -
                    1 / 3)), a]
			}, f(c, function (s, n) {
			    var a = n.props,
                    o = n.cache,
                    h = n.to,
                    c = n.from;
			    l.fn[s] = function (s) {
			        if (h && !this[o] && (this[o] = h(
                        this._rgba)), s === e) return this[
                        o].slice();
			        var n, r = t.type(s),
                        u = "array" === r || "object" ===
                            r ? s : arguments,
                        d = this[o].slice();
			        return f(a, function (t, e) {
			            var s = u["object" === r ? t :
                            e.idx];
			            null == s && (s = d[e.idx]), d[
                            e.idx] = i(s, e)
			        }), c ? (n = l(c(d)), n[o] = d, n) :
                        l(d)
			    }, f(a, function (e, i) {
			        l.fn[e] || (l.fn[e] = function (
                        n) {
			            var a, o = t.type(n),
                            h = "alpha" === e ? this._hsla ?
                                "hsla" : "rgba" : s,
                            l = this[h](),
                            c = l[i.idx];
			            return "undefined" === o ? c :
                            ("function" === o && (n = n.call(
                                this, c), o = t.type(n)),
                            null == n && i.empty ? this :
                            ("string" === o && (a = r.exec(
                                    n), a && (n = c +
                                    parseFloat(a[2]) * ("+" ===
                                        a[1] ? 1 : -1))), l[i.idx] =
                                n, this[h](l)))
			        })
			    })
			}), l.hook = function (e) {
			    var i = e.split(" ");
			    f(i, function (e, i) {
			        t.cssHooks[i] = {
			            set: function (e, n) {
			                var a, o, r = "";
			                if ("transparent" !== n && (
                                "string" !== t.type(n) || (a =
                                    s(n)))) {
			                    if (n = l(a || n), !d.rgba &&
                                    1 !== n._rgba[3]) {
			                        for (o = "backgroundColor" ===
                                        i ? e.parentNode : e;
                                        ("" === r || "transparent" ===
                                            r) && o && o.style;) try {
                                                r = t.css(o,
                                                    "backgroundColor"), o = o.parentNode
                                            } catch (h) { }
			                        n = n.blend(r &&
                                        "transparent" !== r ? r :
                                        "_default")
			                    }
			                    n = n.toRgbaString()
			                }
			                try {
			                    e.style[i] = n
			                } catch (h) { }
			            }
			        }, t.fx.step[i] = function (e) {
			            e.colorInit || (e.start = l(e.elem,
                                i), e.end = l(e.end), e.colorInit = !
                            0), t.cssHooks[i].set(e.elem, e
                            .start.transition(e.end, e.pos)
                        )
			        }
			    })
			}, l.hook(o), t.cssHooks.borderColor = {
			    expand: function (t) {
			        var e = {};
			        return f(["Top", "Right", "Bottom",
                        "Left"
			        ], function (i, s) {
			            e["border" + s + "Color"] = t
			        }), e
			    }
			}, a = t.Color.names = {
			    aqua: "#00ffff",
			    black: "#000000",
			    blue: "#0000ff",
			    fuchsia: "#ff00ff",
			    gray: "#808080",
			    green: "#008000",
			    lime: "#00ff00",
			    maroon: "#800000",
			    navy: "#000080",
			    olive: "#808000",
			    purple: "#800080",
			    red: "#ff0000",
			    silver: "#c0c0c0",
			    teal: "#008080",
			    white: "#ffffff",
			    yellow: "#ffff00",
			    transparent: [null, null, null, 0],
			    _default: "#ffffff"
			}
	}(jQuery),
	function () {
	    function i(e) {
	        var i, s, n = e.ownerDocument.defaultView ?
					e.ownerDocument.defaultView.getComputedStyle(
						e, null) : e.currentStyle,
				a = {};
	        if (n && n.length && n[0] && n[n[0]])
	            for (s = n.length; s--;) i = n[s],
					"string" == typeof n[i] && (a[t.camelCase(
						i)] = n[i]);
	        else
	            for (i in n) "string" == typeof n[
					i] && (a[i] = n[i]);
	        return a
	    }

	    function s(e, i) {
	        var s, n, o = {};
	        for (s in i) n = i[s], e[s] !== n &&
				(a[s] || (t.fx.step[s] || !isNaN(
				parseFloat(n))) && (o[s] = n));
	        return o
	    }
	    var n = ["add", "remove", "toggle"],
			a = {
			    border: 1,
			    borderBottom: 1,
			    borderColor: 1,
			    borderLeft: 1,
			    borderRight: 1,
			    borderTop: 1,
			    borderWidth: 1,
			    margin: 1,
			    padding: 1
			};
	    t.each(["borderLeftStyle",
			"borderRightStyle",
			"borderBottomStyle",
			"borderTopStyle"
	    ], function (e, i) {
	        t.fx.step[i] = function (t) {
	            ("none" !== t.end && !t.setAttr ||
					1 === t.pos && !t.setAttr) && (
					jQuery.style(t.elem, i, t.end),
					t.setAttr = !0)
	        }
	    }), t.fn.addBack || (t.fn.addBack =
			function (t) {
			    return this.add(null == t ? this.prevObject :
					this.prevObject.filter(t))
			}), t.effects.animateClass =
			function (e, a, o, r) {
			    var h = t.speed(a, o, r);
			    return this.queue(function () {
			        var a, o = t(this),
						r = o.attr("class") || "",
						l = h.children ? o.find("*").addBack() :
							o;
			        l = l.map(function () {
			            var e = t(this);
			            return {
			                el: e,
			                start: i(this)
			            }
			        }), a = function () {
			            t.each(n, function (t, i) {
			                e[i] && o[i + "Class"](e[i])
			            })
			        }, a(), l = l.map(function () {
			            return this.end = i(this.el[0]),
							this.diff = s(this.start,
								this.end), this
			        }), o.attr("class", r), l = l.map(
						function () {
						    var e = this,
								i = t.Deferred(),
								s = t.extend({}, h, {
								    queue: !1,
								    complete: function () {
								        i.resolve(e)
								    }
								});
						    return this.el.animate(this.diff,
								s), i.promise()
						}), t.when.apply(t, l.get()).done(
						function () {
						    a(), t.each(arguments,
								function () {
								    var e = this.el;
								    t.each(this.diff, function (
										t) {
								        e.css(t, "")
								    })
								}), h.complete.call(o[0])
						})
			    })
			}, t.fn.extend({
			    addClass: function (e) {
			        return function (i, s, n, a) {
			            return s ? t.effects.animateClass
                            .call(this, {
                                add: i
                            }, s, n, a) : e.apply(this,
                                arguments)
			        }
			    }(t.fn.addClass),
			    removeClass: function (e) {
			        return function (i, s, n, a) {
			            return arguments.length > 1 ? t.effects
                            .animateClass.call(this, {
                                remove: i
                            }, s, n, a) : e.apply(this,
                                arguments)
			        }
			    }(t.fn.removeClass),
			    toggleClass: function (i) {
			        return function (s, n, a, o, r) {
			            return "boolean" == typeof n ||
                            n === e ? a ? t.effects.animateClass
                            .call(this, n ? {
                                add: s
                            } : {
                                remove: s
                            }, a, o, r) : i.apply(this,
                                arguments) : t.effects.animateClass
                            .call(this, {
                                toggle: s
                            }, n, a, o)
			        }
			    }(t.fn.toggleClass),
			    switchClass: function (e, i, s, n,
                    a) {
			        return t.effects.animateClass.call(
                        this, {
                            add: i,
                            remove: e
                        }, s, n, a)
			    }
			})
	}(),
	function () {
	    function s(e, i, s, n) {
	        return t.isPlainObject(e) && (i = e,
				e = e.effect), e = {
				    effect: e
				}, null == i && (i = {}), t.isFunction(
				i) && (n = i, s = null, i = {}), (
				"number" == typeof i || t.fx.speeds[
					i]) && (n = s, s = i, i = {}), t.isFunction(
				s) && (n = s, s = null), i && t.extend(
				e, i), s = s || i.duration, e.duration =
				t.fx.off ? 0 : "number" == typeof s ?
	            s : s in t.fx.speeds ? t.fx.speeds[
					s] : t.fx.speeds._default, e.complete =
				n || i.complete, e
	    }

	    function n(e) {
	        return !e || "number" == typeof e ||
				t.fx.speeds[e] ? !0 : "string" !=
				typeof e || t.effects.effect[e] ?
				t.isFunction(e) ? !0 : "object" !=
				typeof e || e.effect ? !1 : !0 : !
				0
	    }
	    t.extend(t.effects, {
	        version: "1.10.3",
	        save: function (t, e) {
	            for (var s = 0; e.length > s; s++)
	                null !== e[s] && t.data(i + e[s],
						t[0].style[e[s]])
	        },
	        restore: function (t, s) {
	            var n, a;
	            for (a = 0; s.length > a; a++)
	                null !== s[a] && (n = t.data(i +
						s[a]), n === e && (n = ""), t.css(
						s[a], n))
	        },
	        setMode: function (t, e) {
	            return "toggle" === e && (e = t.is(
					":hidden") ? "show" : "hide"), e
	        },
	        getBaseline: function (t, e) {
	            var i, s;
	            switch (t[0]) {
	                case "top":
	                    i = 0;
	                    break;
	                case "middle":
	                    i = .5;
	                    break;
	                case "bottom":
	                    i = 1;
	                    break;
	                default:
	                    i = t[0] / e.height
	            }
	            switch (t[1]) {
	                case "left":
	                    s = 0;
	                    break;
	                case "center":
	                    s = .5;
	                    break;
	                case "right":
	                    s = 1;
	                    break;
	                default:
	                    s = t[1] / e.width
	            }
	            return {
	                x: s,
	                y: i
	            }
	        },
	        createWrapper: function (e) {
	            if (e.parent().is(
					".ui-effects-wrapper")) return e
					.parent();
	            var i = {
	                width: e.outerWidth(!0),
	                height: e.outerHeight(!0),
	                "float": e.css("float")
	            }, s = t("<div></div>").addClass(
						"ui-effects-wrapper").css({
						    fontSize: "100%",
						    background: "transparent",
						    border: "none",
						    margin: 0,
						    padding: 0
						}),
					n = {
					    width: e.width(),
					    height: e.height()
					}, a = document.activeElement;
	            try {
	                a.id
	            } catch (o) {
	                a = document.body
	            }
	            return e.wrap(s), (e[0] === a ||
					t.contains(e[0], a)) && t(a).focus(),
					s = e.parent(), "static" === e.css(
						"position") ? (s.css({
						    position: "relative"
						}), e.css({
						    position: "relative"
						})) : (t.extend(i, {
						    position: e.css("position"),
						    zIndex: e.css("z-index")
						}), t.each(["top", "left",
						"bottom", "right"
						], function (t, s) {
						    i[s] = e.css(s), isNaN(
                                parseInt(i[s], 10)) && (i[s] =
                                "auto")
						}), e.css({
						    position: "relative",
						    top: 0,
						    left: 0,
						    right: "auto",
						    bottom: "auto"
						})), e.css(n), s.css(i).show()
	        },
	        removeWrapper: function (e) {
	            var i = document.activeElement;
	            return e.parent().is(
					".ui-effects-wrapper") && (e.parent()
					.replaceWith(e), (e[0] === i ||
						t.contains(e[0], i)) && t(i).focus()
				), e
	        },
	        setTransition: function (e, i, s,
				n) {
	            return n = n || {}, t.each(i,
					function (t, i) {
					    var a = e.cssUnit(i);
					    a[0] > 0 && (n[i] = a[0] * s +
							a[1])
					}), n
	        }
	    }), t.fn.extend({
	        effect: function () {
	            function e(e) {
	                function s() {
	                    t.isFunction(a) && a.call(n[0]),
							t.isFunction(e) && e()
	                }
	                var n = t(this),
						a = i.complete,
						r = i.mode;
	                (n.is(":hidden") ? "hide" === r :
						"show" === r) ? (n[r](), s()) :
						o.call(n[0], i, s)
	            }
	            var i = s.apply(this, arguments),
					n = i.mode,
					a = i.queue,
					o = t.effects.effect[i.effect];
	            return t.fx.off || !o ? n ? this[
					n](i.duration, i.complete) :
					this.each(function () {
					    i.complete && i.complete.call(
							this)
					}) : a === !1 ? this.each(e) :
					this.queue(a || "fx", e)
	        },
	        show: function (t) {
	            return function (e) {
	                if (n(e)) return t.apply(this,
						arguments);
	                var i = s.apply(this, arguments);
	                return i.mode = "show", this.effect
						.call(this, i)
	            }
	        }(t.fn.show),
	        hide: function (t) {
	            return function (e) {
	                if (n(e)) return t.apply(this,
						arguments);
	                var i = s.apply(this, arguments);
	                return i.mode = "hide", this.effect
						.call(this, i)
	            }
	        }(t.fn.hide),
	        toggle: function (t) {
	            return function (e) {
	                if (n(e) || "boolean" == typeof e)
	                    return t.apply(this, arguments);
	                var i = s.apply(this, arguments);
	                return i.mode = "toggle", this.effect
						.call(this, i)
	            }
	        }(t.fn.toggle),
	        cssUnit: function (e) {
	            var i = this.css(e),
					s = [];
	            return t.each(["em", "px", "%",
					"pt"
	            ], function (t, e) {
	                i.indexOf(e) > 0 && (s = [
						parseFloat(i), e
	                ])
	            }), s
	        }
	    })
	}(),
	function () {
	    var e = {};
	    t.each(["Quad", "Cubic", "Quart",
			"Quint", "Expo"
	    ], function (t, i) {
	        e[i] = function (e) {
	            return Math.pow(e, t + 2)
	        }
	    }), t.extend(e, {
	        Sine: function (t) {
	            return 1 - Math.cos(t * Math.PI /
					2)
	        },
	        Circ: function (t) {
	            return 1 - Math.sqrt(1 - t * t)
	        },
	        Elastic: function (t) {
	            return 0 === t || 1 === t ? t : -
					Math.pow(2, 8 * (t - 1)) * Math.sin(
						(80 * (t - 1) - 7.5) * Math.PI /
						15)
	        },
	        Back: function (t) {
	            return t * t * (3 * t - 2)
	        },
	        Bounce: function (t) {
	            for (var e, i = 4;
					((e = Math.pow(2, --i)) - 1) /
					11 > t;);
	            return 1 / Math.pow(4, 3 - i) - 7
	            .5625 * Math.pow((3 * e - 2) /
                    22 - t, 2)
	        }
	    }), t.each(e, function (e, i) {
	        t.easing["easeIn" + e] = i, t.easing[
				"easeOut" + e] = function (t) {
				    return 1 - i(1 - t)
				}, t.easing["easeInOut" + e] =
				function (t) {
				    return .5 > t ? i(2 * t) / 2 : 1 -
						i(-2 * t + 2) / 2
				}
	    })
	}()
})(jQuery);
(function (t) {
    var e = /up|down|vertical/,
		i = /up|left|vertical|horizontal/;
    t.effects.effect.blind = function (s,
		n) {
        var a, o, r, h = t(this),
			l = ["position", "top", "bottom",
				"left", "right", "height", "width"
			],
			c = t.effects.setMode(h, s.mode ||
				"hide"),
			u = s.direction || "up",
			d = e.test(u),
			p = d ? "height" : "width",
			f = d ? "top" : "left",
			m = i.test(u),
			g = {}, v = "show" === c;
        h.parent().is(".ui-effects-wrapper") ?
			t.effects.save(h.parent(), l) : t.effects
			.save(h, l), h.show(), a = t.effects
			.createWrapper(h).css({
			    overflow: "hidden"
			}), o = a[p](), r = parseFloat(a.css(
				f)) || 0, g[p] = v ? o : 0, m || (
				h.css(d ? "bottom" : "right", 0).css(
					d ? "top" : "left", "auto").css({
					    position: "absolute"
					}), g[f] = v ? r : o + r), v && (a
				.css(p, 0), m || a.css(f, r + o)),
			a.animate(g, {
			    duration: s.duration,
			    easing: s.easing,
			    queue: !1,
			    complete: function () {
			        "hide" === c && h.hide(), t.effects
						.restore(h, l), t.effects.removeWrapper(
							h), n()
			    }
			})
    }
})(jQuery);
(function (t) {
    t.effects.effect.bounce = function (e,
		i) {
        var s, n, a, o = t(this),
			r = ["position", "top", "bottom",
				"left", "right", "height", "width"
			],
			h = t.effects.setMode(o, e.mode ||
				"effect"),
			l = "hide" === h,
			c = "show" === h,
			u = e.direction || "up",
			d = e.distance,
			p = e.times || 5,
			f = 2 * p + (c || l ? 1 : 0),
			m = e.duration / f,
			g = e.easing,
			v = "up" === u || "down" === u ?
				"top" : "left",
			_ = "up" === u || "left" === u,
			b = o.queue(),
			y = b.length;
        for ((c || l) && r.push("opacity"),
			t.effects.save(o, r), o.show(), t.effects
			.createWrapper(o), d || (d = o[
				"top" === v ? "outerHeight" :
				"outerWidth"]() / 3), c && (a = {
            opacity: 1
        }, a[v] = 0, o.css("opacity", 0).css(
				v, _ ? 2 * -d : 2 * d).animate(a,
				m, g)), l && (d /= Math.pow(2, p -
				1)), a = {}, a[v] = 0, s = 0; p >
			s; s++) n = {}, n[v] = (_ ? "-=" :
			"+=") + d, o.animate(n, m, g).animate(
			a, m, g), d = l ? 2 * d : d / 2;
        l && (n = {
            opacity: 0
        }, n[v] = (_ ? "-=" : "+=") + d, o.animate(
			n, m, g)), o.queue(function () {
			    l && o.hide(), t.effects.restore(o,
                    r), t.effects.removeWrapper(o), i()
			}), y > 1 && b.splice.apply(b, [1, 0]
			.concat(b.splice(y, f + 1))), o.dequeue()
    }
})(jQuery);
(function (t) {
    t.effects.effect.clip = function (e,
		i) {
        var s, n, a, o = t(this),
			r = ["position", "top", "bottom",
				"left", "right", "height", "width"
			],
			h = t.effects.setMode(o, e.mode ||
				"hide"),
			l = "show" === h,
			c = e.direction || "vertical",
			u = "vertical" === c,
			d = u ? "height" : "width",
			p = u ? "top" : "left",
			f = {};
        t.effects.save(o, r), o.show(), s =
			t.effects.createWrapper(o).css({
			    overflow: "hidden"
			}), n = "IMG" === o[0].tagName ? s :
			o, a = n[d](), l && (n.css(d, 0), n
				.css(p, a / 2)), f[d] = l ? a : 0,
			f[p] = l ? 0 : a / 2, n.animate(f, {
			    queue: !1,
			    duration: e.duration,
			    easing: e.easing,
			    complete: function () {
			        l || o.hide(), t.effects.restore(
						o, r), t.effects.removeWrapper(
						o), i()
			    }
			})
    }
})(jQuery);
(function (t) {
    t.effects.effect.drop = function (e,
		i) {
        var s, n = t(this),
			a = ["position", "top", "bottom",
				"left", "right", "opacity",
				"height", "width"
			],
			o = t.effects.setMode(n, e.mode ||
				"hide"),
			r = "show" === o,
			h = e.direction || "left",
			l = "up" === h || "down" === h ?
				"top" : "left",
			c = "up" === h || "left" === h ?
				"pos" : "neg",
			u = {
			    opacity: r ? 1 : 0
			};
        t.effects.save(n, a), n.show(), t.effects
			.createWrapper(n), s = e.distance ||
			n["top" === l ? "outerHeight" :
				"outerWidth"](!0) / 2, r && n.css(
				"opacity", 0).css(l, "pos" === c ? -
            s : s), u[l] = (r ? "pos" === c ?
				"+=" : "-=" : "pos" === c ? "-=" :
				"+=") + s, n.animate(u, {
				    queue: !1,
				    duration: e.duration,
				    easing: e.easing,
				    complete: function () {
				        "hide" === o && n.hide(), t.effects
                            .restore(n, a), t.effects.removeWrapper(
                                n), i()
				    }
				})
    }
})(jQuery);
(function (t) {
    t.effects.effect.explode = function (
		e, i) {
        function s() {
            b.push(this), b.length === u * d &&
				n()
        }

        function n() {
            p.css({
                visibility: "visible"
            }), t(b).remove(), m || p.hide(), i()
        }
        var a, o, r, h, l, c, u = e.pieces ?
				Math.round(Math.sqrt(e.pieces)) :
				3,
			d = u,
			p = t(this),
			f = t.effects.setMode(p, e.mode ||
				"hide"),
			m = "show" === f,
			g = p.show().css("visibility",
				"hidden").offset(),
			v = Math.ceil(p.outerWidth() / d),
			_ = Math.ceil(p.outerHeight() / u),
			b = [];
        for (a = 0; u > a; a++)
            for (h = g.top + a * _, c = a - (u -
				1) / 2, o = 0; d > o; o++) r = g.left +
				o * v, l = o - (d - 1) / 2, p.clone()
				.appendTo("body").wrap(
					"<div></div>").css({
					    position: "absolute",
					    visibility: "visible",
					    left: -o * v,
					    top: -a * _
					}).parent().addClass(
					"ui-effects-explode").css({
					    position: "absolute",
					    overflow: "hidden",
					    width: v,
					    height: _,
					    left: r + (m ? l * v : 0),
					    top: h + (m ? c * _ : 0),
					    opacity: m ? 0 : 1
					}).animate({
					    left: r + (m ? 0 : l * v),
					    top: h + (m ? 0 : c * _),
					    opacity: m ? 1 : 0
					}, e.duration || 500, e.easing, s)
    }
})(jQuery);
(function (t) {
    t.effects.effect.fade = function (e,
		i) {
        var s = t(this),
			n = t.effects.setMode(s, e.mode ||
				"toggle");
        s.animate({
            opacity: n
        }, {
            queue: !1,
            duration: e.duration,
            easing: e.easing,
            complete: i
        })
    }
})(jQuery);
(function (t) {
    t.effects.effect.fold = function (e,
		i) {
        var s, n, a = t(this),
			o = ["position", "top", "bottom",
				"left", "right", "height", "width"
			],
			r = t.effects.setMode(a, e.mode ||
				"hide"),
			h = "show" === r,
			l = "hide" === r,
			c = e.size || 15,
			u = /([0-9]+)%/.exec(c),
			d = !!e.horizFirst,
			p = h !== d,
			f = p ? ["width", "height"] : [
				"height", "width"
			],
			m = e.duration / 2,
			g = {}, v = {};
        t.effects.save(a, o), a.show(), s =
			t.effects.createWrapper(a).css({
			    overflow: "hidden"
			}), n = p ? [s.width(), s.height()] : [
				s.height(), s.width()
			], u && (c = parseInt(u[1], 10) /
			100 * n[l ? 0 : 1]), h && s.css(d ? {
			    height: 0,
			    width: c
			} : {
			    height: c,
			    width: 0
			}), g[f[0]] = h ? n[0] : c, v[f[1]] =
			h ? n[1] : 0, s.animate(g, m, e.easing)
			.animate(v, m, e.easing, function () {
			    l && a.hide(), t.effects.restore(
					a, o), t.effects.removeWrapper(a),
					i()
			})
    }
})(jQuery);
(function (t) {
    t.effects.effect.highlight = function (
		e, i) {
        var s = t(this),
			n = ["backgroundImage",
				"backgroundColor", "opacity"
			],
			a = t.effects.setMode(s, e.mode ||
				"show"),
			o = {
			    backgroundColor: s.css(
					"backgroundColor")
			};
        "hide" === a && (o.opacity = 0), t.effects
			.save(s, n), s.show().css({
			    backgroundImage: "none",
			    backgroundColor: e.color ||
					"#ffff99"
			}).animate(o, {
			    queue: !1,
			    duration: e.duration,
			    easing: e.easing,
			    complete: function () {
			        "hide" === a && s.hide(), t.effects
						.restore(s, n), i()
			    }
			})
    }
})(jQuery);
(function (t) {
    t.effects.effect.pulsate = function (
		e, i) {
        var s, n = t(this),
			a = t.effects.setMode(n, e.mode ||
				"show"),
			o = "show" === a,
			r = "hide" === a,
			h = o || "hide" === a,
			l = 2 * (e.times || 5) + (h ? 1 : 0),
			c = e.duration / l,
			u = 0,
			d = n.queue(),
			p = d.length;
        for ((o || !n.is(":visible")) && (n.css(
				"opacity", 0).show(), u = 1), s =
			1; l > s; s++) n.animate({
			    opacity: u
			}, c, e.easing), u = 1 - u;
        n.animate({
            opacity: u
        }, c, e.easing), n.queue(function () {
            r && n.hide(), i()
        }), p > 1 && d.splice.apply(d, [1, 0]
			.concat(d.splice(p, l + 1))), n.dequeue()
    }
})(jQuery);
(function (t) {
    t.effects.effect.puff = function (e,
		i) {
        var s = t(this),
			n = t.effects.setMode(s, e.mode ||
				"hide"),
			a = "hide" === n,
			o = parseInt(e.percent, 10) || 150,
			r = o / 100,
			h = {
			    height: s.height(),
			    width: s.width(),
			    outerHeight: s.outerHeight(),
			    outerWidth: s.outerWidth()
			};
        t.extend(e, {
            effect: "scale",
            queue: !1,
            fade: !0,
            mode: n,
            complete: i,
            percent: a ? o : 100,
            from: a ? h : {
                height: h.height * r,
                width: h.width * r,
                outerHeight: h.outerHeight * r,
                outerWidth: h.outerWidth * r
            }
        }), s.effect(e)
    }, t.effects.effect.scale = function (
		e, i) {
        var s = t(this),
			n = t.extend(!0, {}, e),
			a = t.effects.setMode(s, e.mode ||
				"effect"),
			o = parseInt(e.percent, 10) || (0 ===
				parseInt(e.percent, 10) ? 0 :
				"hide" === a ? 0 : 100),
			r = e.direction || "both",
			h = e.origin,
			l = {
			    height: s.height(),
			    width: s.width(),
			    outerHeight: s.outerHeight(),
			    outerWidth: s.outerWidth()
			}, c = {
			    y: "horizontal" !== r ? o / 100 : 1,
			    x: "vertical" !== r ? o / 100 : 1
			};
        n.effect = "size", n.queue = !1, n.complete =
			i, "effect" !== a && (n.origin = h || [
				"middle", "center"
			], n.restore = !0), n.from = e.from ||
			("show" === a ? {
			    height: 0,
			    width: 0,
			    outerHeight: 0,
			    outerWidth: 0
			} : l), n.to = {
			    height: l.height * c.y,
			    width: l.width * c.x,
			    outerHeight: l.outerHeight * c.y,
			    outerWidth: l.outerWidth * c.x
			}, n.fade && ("show" === a && (n.from
				.opacity = 0, n.to.opacity = 1),
			"hide" === a && (n.from.opacity = 1,
				n.to.opacity = 0)), s.effect(n)
    }, t.effects.effect.size = function (
		e, i) {
        var s, n, a, o = t(this),
			r = ["position", "top", "bottom",
				"left", "right", "width", "height",
				"overflow", "opacity"
			],
			h = ["position", "top", "bottom",
				"left", "right", "overflow",
				"opacity"
			],
			l = ["width", "height", "overflow"],
			c = ["fontSize"],
			u = ["borderTopWidth",
				"borderBottomWidth", "paddingTop",
				"paddingBottom"
			],
			d = ["borderLeftWidth",
				"borderRightWidth", "paddingLeft",
				"paddingRight"
			],
			p = t.effects.setMode(o, e.mode ||
				"effect"),
			f = e.restore || "effect" !== p,
			m = e.scale || "both",
			g = e.origin || ["middle", "center"],
			v = o.css("position"),
			_ = f ? r : h,
			b = {
			    height: 0,
			    width: 0,
			    outerHeight: 0,
			    outerWidth: 0
			};
        "show" === p && o.show(), s = {
            height: o.height(),
            width: o.width(),
            outerHeight: o.outerHeight(),
            outerWidth: o.outerWidth()
        }, "toggle" === e.mode && "show" ===
			p ? (o.from = e.to || b, o.to = e.from ||
				s) : (o.from = e.from || ("show" ===
				p ? b : s), o.to = e.to || (
				"hide" === p ? b : s)), a = {
				    from: {
				        y: o.from.height / s.height,
				        x: o.from.width / s.width
				    },
				    to: {
				        y: o.to.height / s.height,
				        x: o.to.width / s.width
				    }
				}, ("box" === m || "both" === m) &&
			(a.from.y !== a.to.y && (_ = _.concat(
				u), o.from = t.effects.setTransition(
				o, u, a.from.y, o.from), o.to = t
			.effects.setTransition(o, u, a.to.y,
				o.to)), a.from.x !== a.to.x && (_ =
			_.concat(d), o.from = t.effects.setTransition(
				o, d, a.from.x, o.from), o.to = t
			.effects.setTransition(o, d, a.to.x,
				o.to))), ("content" === m ||
			"both" === m) && a.from.y !== a.to.y &&
			(_ = _.concat(c).concat(l), o.from =
			t.effects.setTransition(o, c, a.from
				.y, o.from), o.to = t.effects.setTransition(
				o, c, a.to.y, o.to)), t.effects.save(
			o, _), o.show(), t.effects.createWrapper(
			o), o.css("overflow", "hidden").css(
			o.from), g && (n = t.effects.getBaseline(
				g, s), o.from.top = (s.outerHeight -
				o.outerHeight()) * n.y, o.from.left =
			(s.outerWidth - o.outerWidth()) * n
			.x, o.to.top = (s.outerHeight - o.to
				.outerHeight) * n.y, o.to.left = (
				s.outerWidth - o.to.outerWidth) *
			n.x), o.css(o.from), ("content" ===
			m || "both" === m) && (u = u.concat(
			["marginTop", "marginBottom"]).concat(
			c), d = d.concat(["marginLeft",
			"marginRight"
			]), l = r.concat(u).concat(d), o.find(
			"*[width]").each(function () {
			    var i = t(this),
                    s = {
                        height: i.height(),
                        width: i.width(),
                        outerHeight: i.outerHeight(),
                        outerWidth: i.outerWidth()
                    };
			    f && t.effects.save(i, l), i.from = {
			        height: s.height * a.from.y,
			        width: s.width * a.from.x,
			        outerHeight: s.outerHeight * a.from
                        .y,
			        outerWidth: s.outerWidth * a.from
                        .x
			    }, i.to = {
			        height: s.height * a.to.y,
			        width: s.width * a.to.x,
			        outerHeight: s.height * a.to.y,
			        outerWidth: s.width * a.to.x
			    }, a.from.y !== a.to.y && (i.from =
                    t.effects.setTransition(i, u, a.from
                        .y, i.from), i.to = t.effects.setTransition(
                        i, u, a.to.y, i.to)), a.from.x !==
                    a.to.x && (i.from = t.effects.setTransition(
                            i, d, a.from.x, i.from), i.to =
                        t.effects.setTransition(i, d, a
                            .to.x, i.to)), i.css(i.from),
                    i.animate(i.to, e.duration, e.easing,
                        function () {
                            f && t.effects.restore(i, l)
                        })
			})), o.animate(o.to, {
			    queue: !1,
			    duration: e.duration,
			    easing: e.easing,
			    complete: function () {
			        0 === o.to.opacity && o.css(
                        "opacity", o.from.opacity),
                        "hide" === p && o.hide(), t.effects
                        .restore(o, _), f || ("static" ===
                            v ? o.css({
                                position: "relative",
                                top: o.to.top,
                                left: o.to.left
                            }) : t.each(["top", "left"],
                                function (t, e) {
                                    o.css(e, function (e, i) {
                                        var s = parseInt(i, 10),
                                            n = t ? o.to.left : o.to.top;
                                        return "auto" === i ? n +
                                            "px" : s + n + "px"
                                    })
                                })), t.effects.removeWrapper(o),
                        i()
			    }
			})
    }
})(jQuery);
(function (t) {
    t.effects.effect.shake = function (e,
		i) {
        var s, n = t(this),
			a = ["position", "top", "bottom",
				"left", "right", "height", "width"
			],
			o = t.effects.setMode(n, e.mode ||
				"effect"),
			r = e.direction || "left",
			h = e.distance || 20,
			l = e.times || 3,
			c = 2 * l + 1,
			u = Math.round(e.duration / c),
			d = "up" === r || "down" === r ?
				"top" : "left",
			p = "up" === r || "left" === r,
			f = {}, m = {}, g = {}, v = n.queue(),
			_ = v.length;
        for (t.effects.save(n, a), n.show(),
			t.effects.createWrapper(n), f[d] =
			(p ? "-=" : "+=") + h, m[d] = (p ?
				"+=" : "-=") + 2 * h, g[d] = (p ?
				"-=" : "+=") + 2 * h, n.animate(f,
				u, e.easing), s = 1; l > s; s++) n
			.animate(m, u, e.easing).animate(g,
				u, e.easing);
        n.animate(m, u, e.easing).animate(f,
			u / 2, e.easing).queue(function () {
			    "hide" === o && n.hide(), t.effects
                    .restore(n, a), t.effects.removeWrapper(
                        n), i()
			}), _ > 1 && v.splice.apply(v, [1, 0]
			.concat(v.splice(_, c + 1))), n.dequeue()
    }
})(jQuery);
(function (t) {
    t.effects.effect.slide = function (e,
		i) {
        var s, n = t(this),
			a = ["position", "top", "bottom",
				"left", "right", "width", "height"
			],
			o = t.effects.setMode(n, e.mode ||
				"show"),
			r = "show" === o,
			h = e.direction || "left",
			l = "up" === h || "down" === h ?
				"top" : "left",
			c = "up" === h || "left" === h,
			u = {};
        t.effects.save(n, a), n.show(), s =
			e.distance || n["top" === l ?
				"outerHeight" : "outerWidth"](!0),
			t.effects.createWrapper(n).css({
			    overflow: "hidden"
			}), r && n.css(l, c ? isNaN(s) ?
				"-" + s : -s : s), u[l] = (r ? c ?
				"+=" : "-=" : c ? "-=" : "+=") + s,
			n.animate(u, {
			    queue: !1,
			    duration: e.duration,
			    easing: e.easing,
			    complete: function () {
			        "hide" === o && n.hide(), t.effects
						.restore(n, a), t.effects.removeWrapper(
							n), i()
			    }
			})
    }
})(jQuery);
(function (t) {
    t.effects.effect.transfer = function (
		e, i) {
        var s = t(this),
			n = t(e.to),
			a = "fixed" === n.css("position"),
			o = t("body"),
			r = a ? o.scrollTop() : 0,
			h = a ? o.scrollLeft() : 0,
			l = n.offset(),
			c = {
			    top: l.top - r,
			    left: l.left - h,
			    height: n.innerHeight(),
			    width: n.innerWidth()
			}, u = s.offset(),
			d = t(
				"<div class='ui-effects-transfer'></div>"
			).appendTo(document.body).addClass(
				e.className).css({
				    top: u.top - r,
				    left: u.left - h,
				    height: s.innerHeight(),
				    width: s.innerWidth(),
				    position: a ? "fixed" : "absolute"
				}).animate(c, e.duration, e.easing,
				function () {
				    d.remove(), i()
				})
    }
})(jQuery);