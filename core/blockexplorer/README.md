# Block explorer

This is a blockexplorer built on top of the nako API.

It is written in react and requires that build.bat or build.sh is run in the blockexplorer directory.

This runs something like:-
```
yarn install
yarn build
copy build/* ../wwwroot/*
```

Therefore the packaged html and assets ends up in wwwroot and ready to serve.