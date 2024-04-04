"use client"
import { useState } from "react"
import { Input } from "@/components/ui/input"
import Image from "next/image"
import { Button } from "@/components/ui/button"

export function Search() {
  const [showFilters, setShowFilters] = useState(false)

  const handleFilterClick = () => {
    setShowFilters(!showFilters)
  }

  return (
    <div className="relative">
      <div className="flex items-center">
        <Input
          type="search"
          placeholder="Search"
          className="w-[500px] pl-10 pr-4 rounded-2xl bg-zinc-100 focus:bg-white"
        />
        <div className="absolute left-0 flex items-center pl-4">
          <Image src="/search.svg" alt="Search Icon" width={20} height={20} />
        </div>
        
        <div className="ml-2 flex items-center">
          <div
            className="px-2.5 py-1.5 rounded-xl hover:bg-zinc-100 cursor-pointer"
            onClick={handleFilterClick}
          >
            <Image src="/filter2.svg" alt="Filter Icon" width={20} height={20} />
          </div>
        </div>
      </div>
      {showFilters && (
        <div className="absolute top-1/2 left-[calc(550px+2rem)] transform -translate-y-1/2 flex items-center">
          <div className="px-2.5 py-1.5 rounded-xl hover:bg-zinc-100">
            <Image src="/sortletter.svg" alt="Sort Letter" width={20} height={20} />
          </div>
          <div className="px-2.5 py-1.5 rounded-xl hover:bg-zinc-100 ml-2">
            <Image src="/sortprice.svg" alt="Sort Price" width={20} height={20} />
          </div>
          <div className="px-2.5 py-1.5 rounded-xl hover:bg-zinc-100 ml-2">
            <Image src="/sortvolume.svg" alt="Sort Volume" width={20} height={20} />
          </div>
        </div>
      )}
    </div>
  )
}