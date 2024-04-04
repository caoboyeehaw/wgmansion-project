"use client";

import { Input } from "@/components/ui/input";
import {
  Command,
  CommandDialog,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
  CommandSeparator,
  CommandShortcut,
} from "@/components/ui/command";
import { useState } from "react";

export function SearchBarAdvanced() {
  const [inputValue, setInputValue] = useState("");

  const handleInputChange = (value: string) => {
    setInputValue(value);
  };

  return (
    <Command>
      <CommandInput
        placeholder="Type a command or search..."
        value={inputValue}
        onValueChange={handleInputChange}
      />
        {inputValue && inputValue.length > 0 && (
        <CommandDialog>
            <CommandList>
            <CommandEmpty>No results found.</CommandEmpty>
            <CommandGroup heading="Suggestions">
              <CommandItem>Calendar</CommandItem>
              <CommandItem>Search Emoji</CommandItem>
              <CommandItem>Calculator</CommandItem>
            </CommandGroup> 
            <CommandSeparator />
            <CommandGroup heading="Settings">
              <CommandItem>Profile</CommandItem>
              <CommandItem>Billing</CommandItem>
              <CommandItem>Settings</CommandItem>
            </CommandGroup>
          </CommandList>
        </CommandDialog>
      )}
    </Command>
  );
}