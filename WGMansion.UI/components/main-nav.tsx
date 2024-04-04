import Link from "next/link"
import { cn } from "@/lib/utils"

export function MainNav({
  className,
  ...props
}: React.HTMLAttributes<HTMLElement>) {
  return (
    <nav
      className={cn("flex items-center space-x-4 lg:space-x-6 font-sans", className)}
      {...props}
    >
      <Link
        href="/botcreation"
        className="text-base font-medium transition-colors hover:text-primary rounded-md bg-teal-100 px-4 py-1.5 hover:bg-teal-200"
      >
        + Create Bot
      </Link>
      <Link
        href="messages"
        className="text-base font-medium text-muted-foreground transition-colors hover:text-primary"
      > 
        Message
      </Link>
      <Link
        href="system"
        className="text-base font-medium text-muted-foreground transition-colors hover:text-primary"
      >
        System
      </Link>
    </nav>
  )
}