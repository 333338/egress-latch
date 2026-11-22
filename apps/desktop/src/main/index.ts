if (process.platform === "win32") await import("./windows.ts");
else if (process.platform === "linux" || process.platform === "darwin") await import("./posix.ts");
else throw new Error("EgressLatch supports Windows, Linux and macOS.");
