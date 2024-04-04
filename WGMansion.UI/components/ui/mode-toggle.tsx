import * as React from "react";
import {
  Tabs,
  TabsContent,
  TabsList,
  TabsTrigger,
} from "@/components/ui/tabs-small";
import { useTheme } from "next-themes";

export function ModeToggle() {
  //const { resolvedTheme, setTheme } = useTheme();

  return (
    <div>
      <Tabs>
        {/*<Tabs value={resolvedTheme}>*/}
        <TabsList>
          <div className="flex items-center space-x-2">
            <TabsTrigger value="light" >
              {/*<TabsTrigger value="light" onClick={() => setTheme("light")}>*/}
              Light
            </TabsTrigger>
            <TabsTrigger value="dark" >
              {/*<TabsTrigger value="dark" onClick={() => setTheme("dark")}>*/}
              Dark
            </TabsTrigger>
            <TabsTrigger value="system">
              {/*<TabsTrigger value="system" onClick={() => setTheme("system")}>*/}
              System
            </TabsTrigger>
          </div>
        </TabsList>
        <TabsContent value="light">
          {/* Light theme content */}
        </TabsContent>
        <TabsContent value="dark">
          {/* Dark theme content */}
        </TabsContent>
        <TabsContent value="system">
          {/* System theme content */}
        </TabsContent>
      </Tabs>
    </div>
  );
}
