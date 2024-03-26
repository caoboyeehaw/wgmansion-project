import Image from "next/image";
import Link from "next/link";

export default function Home() {
  return (
    <div className="justify-between flex">
      <div className="items-center flex">wargame mansion app</div>
      <Link href="/test">
        <div className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
          Go to Test Page
        </div>
      </Link>
    </div>
  );
}