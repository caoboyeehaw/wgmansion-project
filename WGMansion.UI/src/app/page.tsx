"use client";

import React, { useState } from "react";
import Image from "next/image";
import Link from "next/link";
import { HamburgerMenuIcon } from '@radix-ui/react-icons'

const Navbar = () => {
  const [isOpen, setIsOpen] = useState(false);

  const toggleMenu = () => {
    setIsOpen(!isOpen);
  };

  return (
    <nav className="bg-black shadow">
      <div className="max-w-auto mx-auto px-2 sm:px-4 lg:px-6">
        <div className="flex justify-between h-16">
          <div className="flex-shrink-0 flex items-center">

          <div className="ml-2">
              <HamburgerMenuIcon />
            </div>


            <Link href="/">
              <div>
                <Image
                  src="/concrete.svg"
                  alt="Logo"
                  width={40}
                  height={40}
                  className="h-8 w-auto"
                />
              </div>
            </Link>

            <div className="text-white text-xl font-semibold mr-2 ml-2">Stonk Simulator</div>
          </div>
          <div className="hidden sm:ml-6 sm:flex sm:items-center">
            {/* replace these ghetto buttons with shadcn components*/}
            <Link href="/createbot">
              <div className="px-3 py-2 rounded-md text-sm font-medium text-white hover:bg-gray-800">
                Create Bot
              </div>
            </Link>
            <Link href="/messages">
              <div className="px-3 py-2 rounded-md text-sm font-medium text-white hover:bg-gray-800">
                Messages
              </div>
            </Link>
            <Link href="/account">
            <div className="px-3 py-2 rounded-md text-sm font-medium text-white hover:bg-gray-800">
              Account
            </div>
          </Link>
          </div>
          {/* ... */}
        </div>
      </div>

      <div className={`${isOpen ? "block" : "hidden"} sm:hidden`}>
        <div className="pt-2 pb-3 space-y-1">
          <Link href="/">
            <div className="block px-3 py-2 rounded-md text-base font-medium text-white hover:bg-gray-800">
              Home
            </div>
          </Link>
          <Link href="/about">
            <div className="block px-3 py-2 rounded-md text-base font-medium text-white hover:bg-gray-800">
              About
            </div>
          </Link>
          <Link href="/contact">
            <div className="block px-3 py-2 rounded-md text-base font-medium text-white hover:bg-gray-800">
              Contact
            </div>
          </Link>
        </div>
      </div>
    </nav>
  );
};

export default function Home() {
  return (
    <div>
      <Navbar />
      <div className="container mx-auto px-4">
        {/* lower main content goes here */}
      </div>
    </div>
  );
}