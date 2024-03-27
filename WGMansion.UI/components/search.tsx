import { Input } from "@/components/ui/input"

export function Search() {
  return (
    <div>
      <Input
        type="search"
        placeholder="Search"
        className="md:w-[100px] lg:w-[500px]"
      />
    </div>
  )
}