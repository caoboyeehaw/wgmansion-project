import type { AppProps } from "next/app";
import { ThemeProvider } from "next-themes";

import '../styles/globals.css';
import '../public/fonts/proxima-nova.css';
import '../public/fonts/bombardier.css';

function MyApp({ Component, pageProps }: AppProps) {
  return (
    <ThemeProvider attribute="class" defaultTheme="system">
      <Component {...pageProps} />
    </ThemeProvider>
  );
}

export default MyApp;