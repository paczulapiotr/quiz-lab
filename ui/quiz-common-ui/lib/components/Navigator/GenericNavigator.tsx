import { useLocalSyncConsumer } from "@/hooks";
import { SyncReceiveDefinitionNames } from "@/services/types";
import { cleanupSlash } from "@/utility";
import { Route, Routes, useLocation, useNavigate } from "react-router";

interface Props<TMessage> {
  basePath?: string;
  key: string;
  routes: Record<string, JSX.Element>;
  queueName: SyncReceiveDefinitionNames;
  createNavigationPath: (message: TMessage) => string;
}

const GenericNavigator = <TMessage,>({
  queueName,
  basePath,
  key,
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
    navigate(
      cleanupSlash(
        `${locationBasePath}${createNavigationPath(message as TMessage)}`,
      ),
    );
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
