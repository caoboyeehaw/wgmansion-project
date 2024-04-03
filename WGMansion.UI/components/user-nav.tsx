import {
  Avatar,
  AvatarFallback,
  AvatarImage,
} from "@/components/ui/avatar"


import Link from "next/link";

import { Button } from "@/components/ui/button"
import { cn } from "@/lib/utils"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuShortcut,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import Image from "next/image";
import { ModeToggle } from "@/components/ui/mode-toggle";

export function UserNav({ className, ...props }: React.HTMLAttributes<HTMLElement>) {
  return (
    <div className={cn("flex items-center space-x-4 lg:space-x-6 font-sans", className)} {...props}>
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button variant="ghost" className="relative h-10 w-10 rounded-full">
            <Avatar className="h-10 w-10">
              <AvatarImage src="/avatars/01.png" alt="@shadcn" />
              <AvatarFallback>DC</AvatarFallback>
            </Avatar>
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent className="w-60 p-2" align="end" forceMount>
          <DropdownMenuLabel className="font-normal">
            <div className="flex items-center space-x-2">
              <div>
                <p className="text-2xl font-medium leading-none">Dylan Cao</p>
                <p className="text-base mt-1 leading-none text-muted-foreground">
                  user@example.com
                </p>

                <p className="text-xs leading-none text-muted-foreground mt-1 hover:underline">
                  <Link href="/profile" className="text-blue-700">
                    View your profile page
                  </Link>
                </p>
              </div>
            </div>
          </DropdownMenuLabel>
          <DropdownMenuGroup>
            <DropdownMenuItem>
              <div className="flex items-center text-base font-medium">
                <Image
                  src="/account.svg"
                  width={20}
                  height={20}
                  alt="logoutLogo"
                  className="rounded-xl mr-2 font-sans"
                />
                Account
              </div>
              <DropdownMenuShortcut></DropdownMenuShortcut>
            </DropdownMenuItem>
            <DropdownMenuItem>
              <div className="flex items-center text-base font-medium">
                <Image
                  src="/verified.svg"
                  width={20}
                  height={20}
                  alt="logoutLogo"
                  className="rounded-xl mr-2"
                />
                Premium
              </div>
              <DropdownMenuShortcut></DropdownMenuShortcut>
            </DropdownMenuItem>
            <DropdownMenuItem>
            <div className="flex items-center text-base font-medium">
                <Image
                  src="/logout.svg"
                  width={20}
                  height={20}
                  alt="logoutLogo"
                  className="rounded-xl mr-2 "
                />
                Themes
              </div>
              <DropdownMenuShortcut></DropdownMenuShortcut>
            </DropdownMenuItem>
          </DropdownMenuGroup>
          <DropdownMenuSeparator />
          <DropdownMenuGroup>
            <DropdownMenuItem>
              <div className="flex items-center text-base font-medium">
                <Image
                  src="/settings.svg"
                  width={20}
                  height={20}
                  alt="SettingsLogo"  
                  className="rounded-xl mr-2"
                />
                Settings
              </div>
              <DropdownMenuShortcut></DropdownMenuShortcut>
            </DropdownMenuItem>
            <DropdownMenuItem>
              <div className="flex items-center text-base font-medium">
                <Image
                  src="/key.svg"
                  width={20}
                  height={20}
                  alt="SettingsLogo"
                  className="rounded-xl mr-2"
                />
                API Keys
              </div>
              <DropdownMenuShortcut></DropdownMenuShortcut>
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <DropdownMenuItem>
            <div className="flex items-center text-base font-medium text-red-500">
                <Image
                  src="/logout.svg"
                  width={20}
                  height={20}
                  alt="logoutLogo"
                  className="rounded-xl mr-2 "
                />
                Sign out
              </div>
              <DropdownMenuShortcut></DropdownMenuShortcut>
            </DropdownMenuItem>
          </DropdownMenuGroup>
        </DropdownMenuContent>
      </DropdownMenu>
    </div>
  )
}