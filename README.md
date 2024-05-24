# DependencyExample

Application to show difficulty in using `AssemblyLoadContext`s in environments where the caller and callee have the same dependency but with different versions.

## Overview

This solution has three projects:
- `Runner` - console application, with dependency on RestWrapper 3.0.19
- `Module1` - class library, with dependency on RestWrapper 3.0.20
- `Module2` - class library, with dependency on RestWrapper 3.0.18

All three programs attempt to perform a RESTful call to the user-supplied URL, displaying the resultant HTTP status and content-length.

`Runner` first performs this operation itself.  It then instantiates a new `AssemblyLoadContext`, loads `Module1`, and invokes its method to perform the same operation.  It then instantiates another `AssemblyLoadContext`, loads `Module2`, and invokes its method to perform the same operation.

The `bin/Debug/net8.0` outputs of `Module1` and `Module2` are copied into subfolders in the `Runner`'s `bin/Debug/net8.0` output.

