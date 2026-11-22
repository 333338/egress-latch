import type { QueryClient } from "@tanstack/react-query";
import { HeadContent, Outlet, createRootRouteWithContext } from "@tanstack/react-router";
import { ThemeProvider } from "next-themes";

import { Toaster } from "@ip-guard/ui/components/sonner";

import brandIcon from "../../../desktop/assets/tray.png";

interface RouterContext {
  queryClient: QueryClient;
}

const RootLayout = () => (
  <ThemeProvider attribute="class" defaultTheme="system" scriptProps={{ type: "application/json" }}>
    <HeadContent />
    <Outlet />
    <Toaster richColors />
  </ThemeProvider>
);

export const Route = createRootRouteWithContext<RouterContext>()({
  head: () => ({
    meta: [{ title: "EgressLatch · 公网 IPv4 守卫" }],
    links: [{ rel: "icon", type: "image/png", href: brandIcon }],
  }),
  component: RootLayout,
});
