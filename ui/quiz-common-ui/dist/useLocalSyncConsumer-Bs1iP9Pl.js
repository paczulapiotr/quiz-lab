import { useContext as s, useRef as u, useEffect as i } from "react";
import { L as S } from "./context-CoVyjGLq.js";
const f = () => {
  const e = s(S);
  if (e === void 0)
    throw new Error(
      "useLocalSyncService must be used within a LocalSyncServiceProvider"
    );
  return e;
}, L = (e, o, r) => {
  const { onSync: c, offSync: t } = f(), n = u();
  i(() => (n.current = r, c(e, r, o), () => {
    n.current && t(e, o);
  }), [r, o, e, t, c]);
};
export {
  L as a,
  f as u
};
//# sourceMappingURL=useLocalSyncConsumer-Bs1iP9Pl.js.map
