import { useLocalSyncConsumer } from "@/hooks";
import { SyncReceiveDefinitionNames } from "@/services/types";
import { cleanupSlash } from "@/utility";
import { Route, Routes, useLocation, useNavigate } from "react-router";

interface Props<TMessage> {
  basePath?: string;
  identifier: string;
  routes: Record<string, JSX.Element | undefined>;
  queueName: SyncReceiveDefinitionNames;
  createNavigationPath: (message: TMessage) => string | null;
}

const GenericNavigator = <TMessage,>({
  queueName,
  basePath,
  identifier: key,
  createNavigationPath,
  routes,
}: Props<TMessage>) => {
  const navigate = useNavigate();
  const { pathname } = useLocation();
  const pathSection = cleanupSlash(`/${basePath ?? ""}/`);
  const locationBasePath = pathname.includes(pathSection)
    ? pathname.split(pathSection)[0] + pathSection
    : pathname;

  useLocalSyncConsumer(queueName, key, (message) => {
    const path = createNavigationPath(message as TMessage);
    if (path == null) return;
    navigate(cleanupSlash(`${locationBasePath}${path}`));
  });

  return (
    <Routes>
      {Object.entries(routes).map(([path, element]) => (
        <Route key={path} path={path} element={element as JSX.Element} />
      ))}
    </Routes>
  );
};

export default GenericNavigator;
