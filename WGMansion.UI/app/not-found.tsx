import React from "react";
import Image from "next/image";

const NotFoundPage: React.FC = () => {
  return (
    <div className="flex flex-col items-center justify-center min-h-screen ">
      <div className="flex items-center mb-4">
        <div className="mr-2 text-zinc-800 text-5xl font-black font-bombardier italic antialiased">Fund</div>
        <div className="mr-1 text-zinc-800 text-4xl font-bold px-2 py-0.5 bg-pink-200 rounded-lg">Hub</div>
        <div className="text-zinc-800 text-sm font-bold ">
          <sup className="text-zinc-800 text-sm font-bold">.AI</sup>
        </div>
      </div>
      <Image
        src="/concrete.jpg"
        alt="Logo"
        width={512}
        height={512}
        className="h-64 w-64 rounded-full mb-6"
      />
      <h1 className="text-4xl font-bold mb-2">404 Not Found</h1>
      <p className="text-md font-semibold">CEMENT, THAT'S CONCRETE BABY</p>
    </div>
  );
};

export default NotFoundPage;
