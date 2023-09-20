# OBJ File Renderer

![Project Image](docs/obj-file-renderer.png)

A lightweight OBJ file renderer written in C# using the SFML library. This project includes XUnit tests, an SFML-based presentation, manual matrix transformation capabilities, and dependency injection for extensibility. It also features a manual OBJ file parser for maximum control.

## Features

- Rendering of OBJ files
- Manually parsed OBJ files
- XUnit tests for code validation
- SFML-based presentation for 3D visualization
- Manual matrix transformation for manipulating models
- Camera Position Adjustment
- Lambertian light distribution model
- Light Position Adjustment
- Z-Buffer
- Flat Shadowing
- Dependency injection for flexibility and modularity

### Manual Matrix Transformation

For manual matrix transformations and a deeper understanding of coordinate systems and transformations, you can refer to the [LearnOpenGL tutorial](https://learnopengl.com/Getting-started/Coordinate-Systems).

### Camera Position Adjustment

This application also provides users with the ability to dynamically change the camera position.  
For a deeper understanding of camera positioning and the spherical coordinate system, you can refer to the [Wikipedia article on Spherical Coordinate Systems](https://en.wikipedia.org/wiki/Spherical_coordinate_system).

### Flat Shadowing

If you'd like to delve deeper into the concept and implementation of flat shadowing, you can watch this informative video tutorial: [Flat Shadowing Tutorial](https://www.youtube.com/watch?v=kCCsko29pv0&t=463s&ab_channel=OGLDEV).

To run this project, you need to have the following prerequisites installed:

- .NET Core SDK
- SFML.NET
