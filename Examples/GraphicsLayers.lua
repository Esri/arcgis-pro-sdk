project "GraphicsLayers"
   kind "SharedLib"
   language "C#"
   targetdir "bin/%{cfg.buildcfg}"
   dotnetframework "4.8"

   files { "GraphicsLayers/**.cs","GraphicsLayers/**.xaml"}
   
   links {
          
}

   filter "configurations:Debug"
      defines { "DEBUG" }
      --flags { "Symbols" }

   filter "configurations:Release"
      defines { "NDEBUG" }
      optimize "On"
	  
	filter { "platforms:x64" }
	system "Windows"
      architecture "x64"