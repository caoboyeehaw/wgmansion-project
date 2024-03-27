import Image from "next/image";
import Link from "next/link";
import { Metadata } from "next"

import { Button } from "@/components/ui/button"
import { HamburgerMenuIcon, DashboardIcon, FileTextIcon, ReaderIcon, GlobeIcon, MixIcon, CodeIcon, BarChartIcon, CrumpledPaperIcon, CubeIcon, LayersIcon, StackIcon, Pencil1Icon, RocketIcon, PlayIcon, Crosshair1Icon} from '@radix-ui/react-icons'

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"

import {
  Tabs, 
  TabsContent,
  TabsList,
  TabsTrigger,
} from "@/components/ui/tabs"

import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet"

import { CalendarDateRangePicker } from "@/components/date-range-picker"
import { MainNav } from "@/components/main-nav"
import { Overview } from "@/components/overview"
import { RecentSales } from "@/components/recent-sales"
import { Search } from "@/components/search"
import { SearchBarAdvanced } from "@/components/search-advanced"
import TeamSwitcher from "@/components/team-switcher"
import { UserNav } from "@/components/user-nav"

export const metadata: Metadata = {
  title: "Alphavest",
  description: "A Website Built for Trading Simulations.",
}

export default function Home() {
  return (
    <>
      <div className="md:hidden">
        <Image
          src="/examples/dashboard-light.png"
          width={1280}
          height={866}
          alt="Dashboard"
          className="block dark:hidden"
        />
        <Image
          src="/examples/dashboard-dark.png"
          width={1280}
          height={866}
          alt="Dashboard" 
          className="hidden dark:block"
        />
      </div>
      <div className="hidden flex-col md:flex">
        <div className=""> {/*this was originally: <div className="border-b">*/}
        {/*for the one below, you might have to consider dark and light modes*/}
        <div className="flex h-16 items-center px-6 w-full top-0 bg-white/80 backdrop-blur-lg fixed z-50">
          {/*   <div className="flex h-16 items-center px-6 w-full top-0 bg-white/80 backdrop-blur-lg fixed z-50">  */}
          {/*   <flex h-28 items-start pt-4 px-6 w-full top-0 bg-white/80 backdrop-blur-lg fixed z-50">  */}


          <div className ="mr-5">
          
          <Sheet>
            <SheetTrigger>
              <HamburgerMenuIcon className="w-5 h-5" />
            </SheetTrigger>
            <SheetContent>
              <SheetHeader>
                <div className="flex items-center space-x-2 pl-3 mt-1">  
                  <HamburgerMenuIcon className="w-5 h-5 mr-2" />
                  <Image src="/concrete.jpg" width={32} height={32} alt="Logo" className="rounded-xl" />
                  <div   className="mr-4 ml-1 text-lg font-semibold">Alphavest</div>
                </div>
                <div className="mt-3 mb-2"></div>
                
                <SheetTitle className="flex items-center text-base font-medium hover:bg-zinc-100 px-4 py-2 rounded-lg"><DashboardIcon className="mr-4 h-4 w-4" /><span>Dashboard</span></SheetTitle>
                <SheetTitle className="flex items-center text-base font-medium hover:bg-zinc-100 px-4 py-2 rounded-lg"><BarChartIcon className="mr-4 h-4 w-4" /><span>Positions</span></SheetTitle>
                <SheetTitle className="flex items-center text-base font-medium hover:bg-zinc-100 px-4 py-2 rounded-lg"><MixIcon className="mr-4 h-4 w-4" /><span>Configure</span></SheetTitle>
                <SheetTitle className="flex items-center text-base font-medium hover:bg-zinc-100 px-4 py-2 rounded-lg"><CrumpledPaperIcon className="mr-4 h-4 w-4" /><span>Tax/Fees</span></SheetTitle>
                <div className="mt-3 mb-4 border-t border-gray-300"></div>
                <div className="text-base font-semibold mt-2 pl-4">Explore</div>
                <SheetTitle className="flex items-center text-base font-medium hover:bg-zinc-100 px-4 py-2 rounded-lg"><RocketIcon className="mr-4 h-4 w-4" /><span>Trending</span></SheetTitle>
                <SheetTitle className="flex items-center text-base font-medium hover:bg-zinc-100 px-4 py-2 rounded-lg"><LayersIcon className="mr-4 h-4 w-4" /><span>Scanners</span></SheetTitle>
                <SheetTitle className="flex items-center text-base font-medium hover:bg-zinc-100 px-4 py-2 rounded-lg"><CodeIcon className="mr-4 h-4 w-4" /><span>Terminal</span></SheetTitle>
                <div className="mt-3 mb-4 border-t border-gray-300"></div>
                <div className="text-base font-semibold mt-2 pl-4">Research</div>
                <SheetTitle className="flex items-center text-base font-medium hover:bg-zinc-100 px-4 py-2 rounded-lg"><Pencil1Icon className="mr-4 h-4 w-4" /><span>Tutorial</span></SheetTitle>
                <SheetTitle className="flex items-center text-base font-medium hover:bg-zinc-100 px-4 py-2 rounded-lg"><ReaderIcon className="mr-4 h-4 w-4" /><span>Document</span></SheetTitle>
                <div className="mt-3 mb-4 border-t border-gray-300"></div>
                <div className="text-base font-semibold mt-2 pl-4">Perform</div>
                <SheetTitle className="flex items-center text-base font-medium hover:bg-zinc-100 px-4 py-2 rounded-lg"><Crosshair1Icon className="mr-4 h-4 w-4" /><span>Battle Royale</span></SheetTitle>
                <SheetTitle className="flex items-center text-base font-medium hover:bg-zinc-100 px-4 py-2 rounded-lg"><PlayIcon className="mr-4 h-4 w-4" /><span>Backtest</span></SheetTitle>
              </SheetHeader>
              <div className="flex-grow"></div>
              <div className="mt-auto">
                <SheetDescription>
                  <div className="mt-3 mb-4 border-t border-gray-300"></div>  
                  {/*probably want to move this down all the way like the youtube one*/}
                  <ul className="flex items-center">
                    <li>
                      <a href="about" className="text-xs text-zinc-700 font-semibold hover:bg-zinc-100 rounded-md px-1.5 py-1">About</a>
                    </li>
                    <li>
                      <a href="terms" className="text-xs text-zinc-700 font-semibold hover:bg-zinc-100 rounded-md px-1.5 py-1">Terms</a>
                    </li>
                    <li>
                      <a href="contact" className="text-xs text-zinc-700 font-semibold hover:bg-zinc-100 rounded-md px-1.5 py-1">Contact</a>
                    </li>
                  </ul>
                  <ul className="mt-1 flex items-center">
                    <li>
                      <a href="privacy" className="text-xs text-zinc-700 font-semibold hover:bg-zinc-100 rounded-md px-1.5 py-1">Privacy</a>
                    </li>
                    <li>
                      <a href="community" className="text-xs text-zinc-700 font-semibold hover:bg-zinc-100 rounded-md px-1.5 py-1">Community</a>
                    </li>
                  </ul>
                  <p className="text-xs mt-16">© 2024 WGMansion LLC</p>
                </SheetDescription>
              </div>
            </SheetContent>
          </Sheet>

          </div>
            <Image
              src="/concrete.jpg"
              width={32}
              height={32}
              alt="Logo"
              className="rounded-xl"
            />
          <div className = "mr-4 ml-2 text-lg font-semibold">
            Alphavest
          </div>
          {/*<TeamSwitcher />*/}
          
          <div className="flex-1 flex justify-center">
            <Search />
            {/*the search bar advanced is kind of broken rn where id you search, it will crash the whole site   <SearchBarAdvanced/>*/}
          </div>
          <div className="ml-auto flex items-center space-x-4">
          <MainNav className="mx-6" />  
            <UserNav />
          </div>
        </div>
        </div>

        <div className="p-8 pt-16">
        <div className="flex items-center justify-between mb-4">
          <h2 className="text-3xl font-bold tracking-tight">Dashboard</h2>
          <div className="flex items-center space-x-2">
            <CalendarDateRangePicker />
            <Button>Download</Button>
          </div>
        </div>
        <Tabs defaultValue="overview">
          <TabsList>
            <div className="flex items-center justify-between space-y-2 my-16">
            </div>
            <TabsTrigger value="overview">Overview</TabsTrigger>
            <TabsTrigger value="analytics" >
              Analytics
            </TabsTrigger>
            <TabsTrigger value="reports" disabled>
              Reports
            </TabsTrigger>
            <TabsTrigger value="notifications" disabled>
              Notifications
            </TabsTrigger>
          </TabsList>
            <TabsContent value="overview" className="space-y-4">
              <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                <Card>
                  <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                    <CardTitle className="text-sm font-medium">
                      Total Revenue
                    </CardTitle>
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      fill="none"
                      stroke="currentColor"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
                      className="h-4 w-4 text-muted-foreground"
                    >
                      <path d="M12 2v20M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6" />
                    </svg>
                  </CardHeader>
                  <CardContent>
                    <div className="text-2xl font-bold">$95,231.89</div>
                    <p className="text-xs text-muted-foreground">
                      +20.1% from last month
                    </p>
                  </CardContent>
                </Card>
                <Card>
                  <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                    <CardTitle className="text-sm font-medium">
                      Subscriptions
                    </CardTitle>
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      fill="none"
                      stroke="currentColor"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
                      className="h-4 w-4 text-muted-foreground"
                    >
                      <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2" />
                      <circle cx="9" cy="7" r="4" />
                      <path d="M22 21v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75" />
                    </svg>
                  </CardHeader>
                  <CardContent>
                    <div className="text-2xl font-bold">+2350</div>
                    <p className="text-xs text-muted-foreground">
                      +180.1% from last month
                    </p>
                  </CardContent>
                </Card>
                
                <Card>
                  <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                    <CardTitle className="text-sm font-medium">Sales</CardTitle>
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      fill="none"
                      stroke="currentColor"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
                      className="h-4 w-4 text-muted-foreground"
                    >
                      <rect width="20" height="14" x="2" y="5" rx="2" />
                      <path d="M2 10h20" />
                    </svg>
                  </CardHeader>
                  <CardContent>
                    <div className="text-2xl font-bold">+12,234</div>
                    <p className="text-xs text-muted-foreground">
                      +19% from last month
                    </p>
                  </CardContent>
                </Card>
                <Card>
                  <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                    <CardTitle className="text-sm font-medium">
                      Active Now
                    </CardTitle>
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      fill="none"
                      stroke="currentColor"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
                      className="h-4 w-4 text-muted-foreground"
                    >
                      <path d="M22 12h-4l-3 9L9 3l-3 9H2" />
                    </svg>
                  </CardHeader>
                  <CardContent>
                    <div className="text-2xl font-bold">+573</div>
                    <p className="text-xs text-muted-foreground">
                      +201 since last hour
                    </p>
                  </CardContent>
                </Card>
              </div>
              <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-7">
                <Card className="col-span-4">
                  <CardHeader>
                    <CardTitle>Overview</CardTitle>
                  </CardHeader>
                  <CardContent className="pl-2">
                    <Overview />
                  </CardContent>
                </Card>
                <Card className="col-span-3">
                  <CardHeader>
                    <CardTitle>Bot 1: botName</CardTitle>
                    <CardDescription>
                      You made 265 sales this month.
                    </CardDescription>
                  </CardHeader>
                  <CardContent>
                    <RecentSales />
                  </CardContent>
                </Card>
              </div>
              <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-7">
                <Card className="col-span-4">
                  <CardHeader>
                    <CardTitle>Overview</CardTitle>
                  </CardHeader>
                  <CardContent className="pl-2">
                    <Overview />
                  </CardContent>
                </Card>
                <Card className="col-span-3">
                  <CardHeader>
                    <CardTitle>Bot 1: botName</CardTitle>
                    <CardDescription>
                      You made 265 sales this month.
                    </CardDescription>
                  </CardHeader>
                  <CardContent>
                    <RecentSales />
                  </CardContent>
                </Card>
              </div>
              <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-7">
                <Card className="col-span-4">
                  <CardHeader>
                    <CardTitle>Overview</CardTitle>
                  </CardHeader>
                  <CardContent className="pl-2">
                    <Overview />
                  </CardContent>
                </Card>
                <Card className="col-span-3">
                  <CardHeader>
                    <CardTitle>Bot 1: botName</CardTitle>
                    <CardDescription>
                      You made 265 sales this month.
                    </CardDescription>
                  </CardHeader>
                  <CardContent>
                    <RecentSales />
                  </CardContent>
                </Card>
              </div>

              <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-7">
                <Card className="col-span-4">
                  <CardHeader>
                    <CardTitle>Overview</CardTitle>
                  </CardHeader>
                  <CardContent className="pl-2">
                    <Overview />
                  </CardContent>
                </Card>
                <Card className="col-span-3">
                  <CardHeader>
                    <CardTitle>Bot 2: botName</CardTitle>
                    <CardDescription>
                      You made 427 sales this month.
                    </CardDescription>
                  </CardHeader>
                  <CardContent>
                    <RecentSales />
                  </CardContent>
                </Card>
              </div>

              <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-7">
                <Card className="col-span-4">
                  <CardHeader>
                    <CardTitle>Overview</CardTitle>
                  </CardHeader>
                  <CardContent className="pl-2">
                    <Overview />
                  </CardContent>
                </Card>
                <Card className="col-span-3">
                  <CardHeader>
                    <CardTitle>Recent Sales</CardTitle>
                    <CardDescription>
                      You made 265 sales this month.
                    </CardDescription>
                  </CardHeader>
                  <CardContent>
                    <RecentSales />
                  </CardContent>
                </Card>
              </div>
            </TabsContent>

            <TabsContent value="analytics" className="space-y-4">
              <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                <Card>
                  <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                    <CardTitle className="text-sm font-medium">
                      Total Revenue
                    </CardTitle>
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      fill="none"
                      stroke="currentColor"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
                      className="h-4 w-4 text-muted-foreground"
                    >
                      <path d="M12 2v20M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6" />
                    </svg>
                  </CardHeader>
                  <CardContent>
                    <div className="text-2xl font-bold">$45,231.89</div>
                    <p className="text-xs text-muted-foreground">
                      +20.1% from last month
                    </p>
                  </CardContent>
                </Card>
                <Card>
                  <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                    <CardTitle className="text-sm font-medium">
                      Subscriptions
                    </CardTitle>
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      fill="none"
                      stroke="currentColor"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
                      className="h-4 w-4 text-muted-foreground"
                    >
                      <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2" />
                      <circle cx="9" cy="7" r="4" />
                      <path d="M22 21v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75" />
                    </svg>
                  </CardHeader>
                  <CardContent>
                    <div className="text-2xl font-bold">+2350</div>
                    <p className="text-xs text-muted-foreground">
                      +180.1% from last month
                    </p>
                  </CardContent>
                </Card>
                <Card>
                  <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                    <CardTitle className="text-sm font-medium">Sales</CardTitle>
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      fill="none"
                      stroke="currentColor"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
                      className="h-4 w-4 text-muted-foreground"
                    >
                      <rect width="20" height="14" x="2" y="5" rx="2" />
                      <path d="M2 10h20" />
                    </svg>
                  </CardHeader>
                  <CardContent>
                    <div className="text-2xl font-bold">+12,234</div>
                    <p className="text-xs text-muted-foreground">
                      +19% from last month
                    </p>
                  </CardContent>
                </Card>
                <Card>
                  <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                    <CardTitle className="text-sm font-medium">
                      Active Now
                    </CardTitle>
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 24 24"
                      fill="none"
                      stroke="currentColor"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth="2"
                      className="h-4 w-4 text-muted-foreground"
                    >
                      <path d="M22 12h-4l-3 9L9 3l-3 9H2" />
                    </svg>
                  </CardHeader>
                  <CardContent>
                    <div className="text-2xl font-bold">+573</div>
                    <p className="text-xs text-muted-foreground">
                      +201 since last hour
                    </p>
                  </CardContent>
                </Card>
              </div>
              <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-7">
                <Card className="col-span-4">
                  <CardHeader>
                    <CardTitle>Overview</CardTitle>
                  </CardHeader>
                  <CardContent className="pl-2">
                    <Overview />
                  </CardContent>
                </Card>
                <Card className="col-span-3">
                  <CardHeader>
                    <CardTitle>Recent Sales</CardTitle>
                    <CardDescription>
                      You made 265 sales this month.
                    </CardDescription>
                  </CardHeader>
                  <CardContent>
                    <RecentSales />
                  </CardContent>
                </Card>
              </div>
            </TabsContent>
          </Tabs>
        </div>
      </div>
    </>
  );
}



