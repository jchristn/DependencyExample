# DependencyExample

Application to show difficulty in using `AssemblyLoadContext`s in environments where the caller and callee have the same dependency but with different versions.

Refer to Stack Overflow question here: https://stackoverflow.com/questions/78529536/assemblyloadcontext-and-dependency-resolving-when-assemblies-and-the-calling-app/78542909#78542909

**Update 5/28/2024** - the issue has been resolved, and the update is incorporated into this repository.

## Overview

This solution has three projects, each with separate version of `RestWrapper`:
- `Runner` - console application
- `Module1` - class library
- `Module2` - class library

All three programs attempt to perform a RESTful call to the user-supplied URL, displaying the resultant HTTP status and content-length.

`Runner` first performs this operation itself.  It then instantiates a new `AssemblyLoadContext`, loads `Module1`, and invokes its method to perform the same operation.  It then instantiates another `AssemblyLoadContext`, loads `Module2`, and invokes its method to perform the same operation.

The `bin/Debug/net8.0` outputs of `Module1` and `Module2` are copied into subfolders in the `Runner`'s `bin/Debug/net8.0` output.

