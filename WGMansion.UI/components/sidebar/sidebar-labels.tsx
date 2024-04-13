
import { cn } from "@/lib/utils"
"use client"

import { usePathname } from 'next/navigation';
import { Button } from '@/components/ui/button';
import { ButtonStart } from '@/components/ui/button-start';
import { CalendarDateRangePicker } from "@/components/date-range-picker"
import { MainNav } from "@/components/main-nav"
import { Overview } from "@/components/overview"
import { RecentSales } from "@/components/recent-sales"
import { Search } from "@/components/search"
import { SearchBarAdvanced } from "@/components/search-advanced"
import { UserNav } from "@/components/user-nav"
import { Progress } from "@/components/ui/progress"
import { Separator } from "@/components/ui/separator"
import { Sheet, SheetContent, SheetTrigger } from "@/components/ui/sheet"
import { Input } from "@/components/ui/input"
import { Badge } from "@/components/ui/badge"

import Image from 'next/image';
import Link from 'next/link';
import TeamSwitcher from "@/components/team-switcher"


import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from "@/components/ui/breadcrumb"

import {
  ChevronLeft,
  ChevronRight,
  Copy,
  CreditCard,
  File,
  //Home,
  LineChart,
  ListFilter,
  MoreVertical,
  Package,
  Package2,
  PanelLeft,
  //Search,
  Settings,
  ShoppingCart,
  Truck,
  Users2,
} from "lucide-react"

import {
  DropdownMenu,
  DropdownMenuCheckboxItem,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"

import {
  Pagination,
  PaginationContent,
  PaginationItem,
} from "@/components/ui/pagination"

import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"

import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from "@/components/ui/tooltip"

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
  CardFooter,
} from "@/components/ui/card"

import {
  Tabs, 
  TabsContent,
  TabsList,
  TabsTrigger,
} from "@/components/ui/tabs"

export function Sidebar({
  className,
  ...props
}: React.HTMLAttributes<HTMLElement>) {
    const pathname = usePathname();
  return (
    <>
        <a>
        <ButtonStart 
            className={`flex items-center text-lg font-medium px-2.5 py-2 rounded-lg ${
            pathname === '/dashboard' ? 'bg-zinc-200' : 'hover:bg-zinc-100'
            }`}
            variant="ghost"
            >
            <Image
            src="dashboard.svg"
            width={30}
            height={30}
            alt="dashboard"
            className="pr-2"
            />
            <span>Dashboard</span>
        </ButtonStart> 
        </a>

        <ButtonStart 
            className="flex items-center text-lg font-medium hover:bg-zinc-100 px-2.5 py-2 rounded-lg"
            variant={"ghost"}
            >
            <Image
            src="positions.svg"
            width={30}
            height={30}
            alt="positions"
            className="pr-2"
            />
            <a href ="position">Position</a>
        </ButtonStart>

        <ButtonStart 
            className="flex items-center text-lg font-medium hover:bg-zinc-100 px-2.5 py-2 rounded-lg"
            variant={"ghost"}
            >
            <Image
            src="configure.svg"
            width={30}
            height={30}
            alt="configure"
            className="pr-2"
            />
            <a href ="configure">Configure</a>
        </ButtonStart>

        <ButtonStart 
            className="flex items-center text-lg font-medium hover:bg-zinc-100 px-2.5 py-2 rounded-lg"
            variant={"ghost"}
            >
            <Image
            src="taxfees.svg"
            width={30}
            height={30}
            alt="taxfees and slippage"
            className="pr-2"
            />
            <a href ="expenses">Expenses</a>
        </ButtonStart>

        <ButtonStart 
            className="flex items-center text-lg font-medium hover:bg-zinc-100 px-2.5 py-2 rounded-lg"
            variant={"ghost"}
            >
            <Image
            src="trending.svg"
            width={30}
            height={30}
            alt="trending"
            className="pr-2"
            />
            <a href ="trending">Trending</a>
        </ButtonStart>

        <ButtonStart 
            className="flex items-center text-lg font-medium hover:bg-zinc-100 px-2.5 py-2 rounded-lg"
            variant={"ghost"}
            >
            <Image
            src="scanner.svg"
            width={30}
            height={30}
            alt="scanner"
            className="pr-2"
            />
            <a href ="scanners">Scanners</a>
        </ButtonStart>

        <ButtonStart 
            className="flex items-center text-lg font-medium hover:bg-zinc-100 px-2.5 py-2 rounded-lg"
            variant={"ghost"}
            >
            <Image
            src="terminal.svg"
            width={30}
            height={30}
            alt="trending"
            className="pr-2"
            />  
            <a href ="terminal">Terminal</a>
        </ButtonStart>

        <ButtonStart 
            className="flex items-center text-lg font-medium hover:bg-zinc-100 px-2.5 py-2 rounded-lg"
            variant={"ghost"}
            >
            <Image
            src="tutorial.svg"
            width={30}
            height={30}
            alt="tutorial"
            className="pr-2"
            />
            <a href ="tutorial">Tutorial</a>
        </ButtonStart>

        <ButtonStart 
            className="flex items-center text-lg font-medium hover:bg-zinc-100 px-2.5 py-2 rounded-lg"
            variant={"ghost"}
            >
            <Image
            src="document.svg"
            width={30}
            height={30}
            alt="document"
            className="pr-2"
            />
            <a href ="document">Document</a>
        </ButtonStart>

        <ButtonStart 
            className="flex items-center text-lg font-medium hover:bg-zinc-100 px-2.5 py-2 rounded-lg"
            variant={"ghost"}
            >
            <Image
            src="challenge.svg"
            width={30}
            height={30}
            alt="challenge"
            className="pr-2"
            />
            <a href ="challenge">Challenge</a>
        </ButtonStart>

        <ButtonStart 
            className="flex items-center text-lg font-medium hover:bg-zinc-100 px-2.5 py-2 rounded-lg"
            variant={"ghost"}
            >
            <Image
            src="experiment.svg"
            width={30}
            height={30}
            alt="backtest"
            className="pr-2"
            />
            <a href ="backtest">Backtest</a>
        </ButtonStart>
    </>
    )
}
