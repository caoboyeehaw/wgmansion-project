'use client';

import React, { useState } from 'react';

const SlidingBar = () => {
  const [isOpen, setIsOpen] = useState(false);

  const toggleSlidingBar = () => {
    setIsOpen(!isOpen);
  };

  return (
    <div className={`sliding-bar ${isOpen ? 'open' : ''}`}>
      <button className="toggle-button" onClick={toggleSlidingBar}>
        {isOpen ? 'Close' : 'Open'}
      </button>
      <div className="sliding-bar-content">
        {/* Add your sliding bar content here */}
        <h2>Sliding Bar Content</h2>
        <p>This is the content of the sliding bar.</p>
      </div>
      <style jsx>{`
        .sliding-bar {
          position: fixed;
          top: 0;
          right: -250px;
          width: 250px;
          height: 100vh;
          background-color: #f1f1f1;
          transition: right 0.3s ease-in-out;
          z-index: 999;
        }

        .sliding-bar.open {
          right: 0;
        }

        .toggle-button {
          position: absolute;
          top: 20px;
          left: -40px;
          padding: 10px;
          background-color: #f1f1f1;
          border: none;
          cursor: pointer;
        }

        .sliding-bar-content {
          padding: 20px;
        }
      `}</style>
    </div>
  );
};

export default SlidingBar;