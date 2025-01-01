import { useLocalSyncConsumer } from "#/hooks";
import { SyncReceiveDefinitionNames } from "#/services/types";
import { cleanupSlash } from "#/utility";
import { AnimatePresence } from "motion/react";
import { Route, Routes, useLocation, useNavigate } from "react-router";

interface Props<TMessage> {
  disableAnimation?: boolean;
  basePath?: string;
  routes: Record<string, JSX.Element | undefined>;
  queueName: SyncReceiveDefinitionNames;
  createNavigationPath: (message: TMessage) => string | null;
}

const GenericNavigator = <TMessage,>({
  queueName,
  basePath,
  createNavigationPath,
  routes,
  disableAnimation: disabledAnimation,
}: Props<TMessage>) => {
  const navigate = useNavigate();
  const location = useLocation();
  const pathSection = cleanupSlash(`/${basePath ?? ""}/`);
  const locationBasePath = location.pathname.includes(pathSection)
    ? location.pathname.split(pathSection)[0] + pathSection
    : location.pathname;

  useLocalSyncConsumer(queueName, (message) => {
    const path = createNavigationPath(message as TMessage);
    if (path == null) return;
    navigate(cleanupSlash(`${locationBasePath}${path}`));
  });

  const children = (
    <Routes location={location} key={location.pathname}>
      {Object.entries(routes).map(([path, element]) => (
        <Route key={path} path={path} element={element as JSX.Element} />
      ))}
    </Routes>
  );

  return disabledAnimation ? (
    children
  ) : (
    <AnimatePresence initial={false}>{children}</AnimatePresence>
  );
};

export default GenericNavigator;
