import Link from "next/link"

import { cn } from "@/lib/utils"

export function MainNav({
  className,
  ...props
}: React.HTMLAttributes<HTMLElement>) {
  return (
    <nav
      className={cn("flex items-center space-x-4 lg:space-x-6", className)}
      {...props}
    >
      <Link
        href="/examples/dashboard"
        className="text-sm font-medium transition-colors hover:text-primary rounded-lg bg-teal-100 px-3 py-1.5"
      >
        Create Bot+
      </Link>
      <Link
        href=""
        className="text-sm font-medium text-muted-foreground transition-colors hover:text-primary"
      >
        Message
      </Link>
      <Link
        href=""
        className="text-sm font-medium text-muted-foreground transition-colors hover:text-primary"
      >
        Updates
      </Link>
    </nav>
  )
}