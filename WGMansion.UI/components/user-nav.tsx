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
import { Switch } from "@/components/ui/switch"
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
        <DropdownMenuContent className="w-50 p-2 rounded-xl" align="end" forceMount>
  <div className="flex items-center">
    <Avatar className="h-10 w-10">
      <AvatarImage src="/avatars/01.png" alt="@shadcn" />
      <AvatarFallback>DC</AvatarFallback>
    </Avatar>
    <div className="ml-2">
      <div className="flex items-center text-xs font-medium">
        <Image
          src="/verified.svg"
          width={14}
          height={14}
          alt="verified"
          className="rounded-xl mr-1"
        />
        {/*Business*/}
        {/*Premium*/}
        {/*Regular*/}
        Regular Member
      </div>
      <div className="flex items-center mt-1 hover:underline">
        <Image
          src="/person.svg"
          width={14}
          height={14}
          alt="verified"
          className="mr-1"
        />
        <p className="text-xs leading-none text-muted-foreground">
          <Link href="/profile" className="text-blue-700">
            View your profile
          </Link>
        </p>
      </div>
    </div>
  </div>
  <DropdownMenuLabel className="font-normal mt-2">
    <div className="flex items-center space-x-2">
      <div>
        <p className="text-2xl font-medium leading-none">Dylan Cao</p>
        <p className="text-base mt-1 leading-none text-muted-foreground overflow-hidden text-ellipsis whitespace-nowrap">
          @aliasName
        </p>
      </div>
    </div>
          </DropdownMenuLabel>
                
          <DropdownMenuSeparator />
          <DropdownMenuGroup>
            <DropdownMenuItem>
              <div className="flex items-center text-base font-medium">
                <Image
                  src="/manageaccount.svg"
                  width={20}
                  height={20}
                  alt="logoutLogo"
                  className="rounded-xl mr-2 font-sans"
                />
                Account Settings
              </div>
              <DropdownMenuShortcut></DropdownMenuShortcut>
            </DropdownMenuItem>
          </DropdownMenuGroup>
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
                General Settings
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
                API Access Keys
              </div>
              <DropdownMenuShortcut></DropdownMenuShortcut>
            </DropdownMenuItem>
            <DropdownMenuItem>
            <div className="flex items-center text-base font-medium">
                <Image
                  src="/lightmode.svg"
                  width={20}
                  height={20}
                  alt="themeLogo"
                  className="rounded-xl mr-2 "
                />
                Light Mode 
                <Switch className="ml-1"></Switch>
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