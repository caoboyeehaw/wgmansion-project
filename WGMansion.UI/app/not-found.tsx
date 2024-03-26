import React from "react";
import Image from "next/image";

const NotFoundPage: React.FC = () => {
  return (
    <div className="flex flex-col items-center justify-center min-h-screen">
      <Image
        src="/concrete.jpg"
        alt="Logo"
        width={512}
        height={512}
        className="h-64 w-64 rounded-md mb-8"
      />
      <h1 className="text-4xl font-bold mb-4">404 Not Found</h1>
      <p className="text-md">CEMENT, THATS CONCRETE BABY</p>
    </div>
  );
};

export default NotFoundPage;