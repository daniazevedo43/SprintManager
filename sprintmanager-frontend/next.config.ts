import type { NextConfig } from "next";
import path from "path";

const workspaceRoot = path.resolve(__dirname, '../');

const nextConfig: NextConfig = {
  /* config options here */
  turbopack: {
    root: workspaceRoot
  }
};

export default nextConfig;