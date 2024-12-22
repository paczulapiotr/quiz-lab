import { jsx as s, jsxs as D } from "react/jsx-runtime";
import { memo as L, useRef as E, useEffect as p, useState as y, useCallback as b, createContext as k, useMemo as q } from "react";
import { u as F, a as $ } from "./useLocalSyncConsumer-Bs1iP9Pl.js";
import { useNavigate as W, useLocation as j, Routes as C, Route as M } from "react-router";
import "./context-CoVyjGLq.js";
const S = (e) => {
  const a = {}, n = Math.random();
  return n < 0.25 ? (a.top = `-${e}px`, a.left = Math.random() * window.innerWidth + "px") : n < 0.5 ? (a.top = Math.random() * window.innerHeight + "px", a.left = window.innerWidth + "px") : n < 0.75 ? (a.top = window.innerHeight + "px", a.left = Math.random() * window.innerWidth + "px") : (a.top = Math.random() * window.innerHeight + "px", a.left = `-${e}px`), a;
}, N = (e) => {
  const a = e.offsetWidth, n = S(a), o = S(a);
  e.style.top = n.top, e.style.left = n.left;
  const r = 3e4, t = performance.now(), i = (l) => {
    const u = l - t, d = Math.min(u / r, 1);
    e.style.top = `calc(${n.top} + (${parseFloat(o.top) - parseFloat(n.top)}px) * ${d})`, e.style.left = `calc(${n.left} + (${parseFloat(o.left) - parseFloat(n.left)}px) * ${d})`, d < 1 ? requestAnimationFrame(i) : N(e);
  };
  requestAnimationFrame(i);
}, A = "_square_1c3e8_1", O = "_rotate_1c3e8_1", T = {
  square: A,
  rotate: O
}, B = ({ count: e = 1 }) => {
  const a = E([]);
  return p(() => {
    a.current.forEach((n) => {
      n && N(n);
    });
  }, [e]), /* @__PURE__ */ s("div", { children: Array.from({ length: e }).map((n, o) => /* @__PURE__ */ s(
    "div",
    {
      ref: (r) => {
        r && (a.current[o] = r);
      },
      className: T.square
    },
    o
  )) });
}, de = L(B), J = "_latency_16adf_1", H = {
  latency: J
}, ue = () => {
  const [e, a] = y(0), n = E(Date.now()), { connected: o, sendSync: r } = F();
  return p(() => {
    if (!o) return;
    const t = setInterval(() => {
      n.current = Date.now();
    }, 5e3);
    return () => clearInterval(t);
  }, [o, r]), $(
    "Pong",
    "Latency",
    b(async () => {
      const t = Date.now() - n.current;
      a(t);
    }, [])
  ), /* @__PURE__ */ s("span", { className: H.latency, children: o ? `${e}ms` : "---" });
};
function V(e) {
  return e && e.__esModule && Object.prototype.hasOwnProperty.call(e, "default") ? e.default : e;
}
var _ = { exports: {} };
/*!
	Copyright (c) 2018 Jed Watson.
	Licensed under the MIT License (MIT), see
	http://jedwatson.github.io/classnames
*/
var h;
function z() {
  return h || (h = 1, function(e) {
    (function() {
      var a = {}.hasOwnProperty;
      function n() {
        for (var t = "", i = 0; i < arguments.length; i++) {
          var l = arguments[i];
          l && (t = r(t, o(l)));
        }
        return t;
      }
      function o(t) {
        if (typeof t == "string" || typeof t == "number")
          return t;
        if (typeof t != "object")
          return "";
        if (Array.isArray(t))
          return n.apply(null, t);
        if (t.toString !== Object.prototype.toString && !t.toString.toString().includes("[native code]"))
          return t.toString();
        var i = "";
        for (var l in t)
          a.call(t, l) && t[l] && (i = r(i, l));
        return i;
      }
      function r(t, i) {
        return i ? t ? t + " " + i : t + i : t;
      }
      e.exports ? (n.default = n, e.exports = n) : window.classNames = n;
    })();
  }(_)), _.exports;
}
var K = z();
const P = /* @__PURE__ */ V(K), Q = "_tile_15f3g_1", X = "_blue_15f3g_14", Y = "_success_15f3g_20", Z = "_failure_15f3g_23", U = "_selected_15f3g_26", f = {
  tile: Q,
  blue: X,
  success: Y,
  failure: Z,
  selected: U
}, fe = ({
  text: e,
  blue: a = !1,
  selected: n = !1,
  success: o = !1,
  failure: r = !1,
  onClick: t
}) => /* @__PURE__ */ s(
  "div",
  {
    onClick: t,
    className: P(f.tile, {
      [f.blue]: a,
      [f.selected]: n,
      [f.success]: o,
      [f.failure]: r
    }),
    children: /* @__PURE__ */ s("p", { children: e })
  }
), ee = "_timer_xabk6_1", ne = "_green_xabk6_21", te = "_yellow_xabk6_24", ae = "_red_xabk6_27", re = "_display_xabk6_30", g = {
  timer: ee,
  "progress-bar": "_progress-bar_xabk6_7",
  "progress-fill": "_progress-fill_xabk6_16",
  green: ne,
  yellow: te,
  red: ae,
  display: re
}, ge = ({ startSeconds: e, onTimeUp: a }) => {
  const [n, o] = y(e), [r, t] = y(0);
  p(() => {
    if (n > 0) {
      const l = setInterval(() => {
        o((u) => u - 1);
      }, 1e3);
      return () => clearInterval(l);
    } else
      setTimeout(() => {
        a == null || a();
      }, 100);
  }, [n, a]), p(() => {
    const l = e - (n - 1);
    t(l / e * 100);
  }, [n, e]);
  const i = () => r < 33 ? "green" : r < 66 ? "yellow" : "red";
  return /* @__PURE__ */ D("div", { className: g.timer, children: [
    /* @__PURE__ */ s("div", { className: g["progress-bar"], children: /* @__PURE__ */ s(
      "div",
      {
        style: { width: `${r}%` },
        className: P(
          g["progress-fill"],
          g[i()]
        )
      }
    ) }),
    /* @__PURE__ */ s("div", { className: g.display, children: `${Math.floor(n / 60).toString().padStart(2, "0")}:${(n % 60).toString().padStart(2, "0")}` })
  ] });
};
k(void 0);
var c = /* @__PURE__ */ ((e) => (e[e.GameCreated = 1] = "GameCreated", e[e.GameJoined = 2] = "GameJoined", e[e.GameStarting = 3] = "GameStarting", e[e.GameStarted = 4] = "GameStarted", e[e.RulesExplaining = 5] = "RulesExplaining", e[e.RulesExplained = 6] = "RulesExplained", e[e.MiniGameStarting = 7] = "MiniGameStarting", e[e.MiniGameStarted = 8] = "MiniGameStarted", e[e.MiniGameEnding = 9] = "MiniGameEnding", e[e.MiniGameEnded = 10] = "MiniGameEnded", e[e.GameEnding = 11] = "GameEnding", e[e.GameEnded = 12] = "GameEnded", e))(c || {}), m;
(function(e) {
  e[e.Trace = 0] = "Trace", e[e.Debug = 1] = "Debug", e[e.Information = 2] = "Information", e[e.Warning = 3] = "Warning", e[e.Error = 4] = "Error", e[e.Critical = 5] = "Critical", e[e.None = 6] = "None";
})(m || (m = {}));
var G;
(function(e) {
  e[e.Invocation = 1] = "Invocation", e[e.StreamItem = 2] = "StreamItem", e[e.Completion = 3] = "Completion", e[e.StreamInvocation = 4] = "StreamInvocation", e[e.CancelInvocation = 5] = "CancelInvocation", e[e.Ping = 6] = "Ping", e[e.Close = 7] = "Close", e[e.Ack = 8] = "Ack", e[e.Sequence = 9] = "Sequence";
})(G || (G = {}));
var I;
(function(e) {
  e.Disconnected = "Disconnected", e.Connecting = "Connecting", e.Connected = "Connected", e.Disconnecting = "Disconnecting", e.Reconnecting = "Reconnecting";
})(I || (I = {}));
var x;
(function(e) {
  e[e.None = 0] = "None", e[e.WebSockets = 1] = "WebSockets", e[e.ServerSentEvents = 2] = "ServerSentEvents", e[e.LongPolling = 4] = "LongPolling";
})(x || (x = {}));
var w;
(function(e) {
  e[e.Text = 1] = "Text", e[e.Binary = 2] = "Binary";
})(w || (w = {}));
m.Trace, m.Debug, m.Information, m.Information, m.Warning, m.Warning, m.Error, m.Critical, m.None;
const v = (e) => e.replace(/\/{2,}/g, "/"), ie = ({
  queueName: e,
  basePath: a,
  identifier: n,
  createNavigationPath: o,
  routes: r
}) => {
  const t = W(), { pathname: i } = j(), l = v(`/${a ?? ""}/`), u = i.includes(l) ? i.split(l)[0] + l : i;
  return $(e, n, (d) => {
    t(
      v(
        `${u}${o(d)}`
      )
    );
  }), /* @__PURE__ */ s(C, { children: Object.entries(r).map(([d, R]) => /* @__PURE__ */ s(M, { path: d, element: R }, d)) });
}, pe = ({ pages: e, basePath: a }) => {
  const n = b(
    (r) => {
      switch (r == null ? void 0 : r.status) {
        case c.GameCreated:
          return `${r.gameId}/created/`;
        case c.GameJoined:
          return `${r.gameId}/joined/`;
        case c.GameStarting:
          return `${r.gameId}/starting/`;
        case c.RulesExplaining:
          return `${r.gameId}/rules/`;
        case c.MiniGameStarting:
          return `${r.gameId}/minigame/`;
        case c.MiniGameStarted:
          return `${r.gameId}/minigame_play/`;
        case c.MiniGameEnding:
          return `${r.gameId}/minigame_finish/`;
        case c.GameEnding:
          return `${r.gameId}/end/`;
        default:
          return "";
      }
    },
    []
  ), o = q(
    () => ({
      ":gameId/created/*": e[c.GameCreated],
      ":gameId/join/*": e[c.GameJoined],
      ":gameId/starting/*": e[c.GameStarting],
      ":gameId/rules/*": e[c.RulesExplaining],
      ":gameId/minigame/*": e[c.MiniGameStarting],
      ":gameId/minigame_play/*": e[c.MiniGameStarted],
      ":gameId/minigame_finish/*": e[c.MiniGameEnding],
      ":gameId/end/*": e[c.GameEnding]
    }),
    [e]
  );
  return /* @__PURE__ */ s(C, { children: /* @__PURE__ */ s(
    M,
    {
      path: "game/*",
      element: /* @__PURE__ */ s(
        ie,
        {
          basePath: v(a + "/game"),
          identifier: "GameNavigator",
          routes: o,
          queueName: "GameStatusUpdate",
          createNavigationPath: n
        }
      )
    }
  ) });
};
export {
  de as FlyingSquare,
  pe as GameNavigator,
  ie as GenericNavigator,
  ue as Latency,
  fe as Tile,
  ge as Timer
};
//# sourceMappingURL=components.js.map
