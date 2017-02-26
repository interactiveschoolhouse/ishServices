Beyond Compare integration
http://www.scootersoftware.com/support.php?zz=kb_vcs#visualstudio-git

Global git config
http://stackoverflow.com/questions/2114111/where-does-git-config-global-get-written-to

http://stackoverflow.com/questions/7231608/how-to-ignore-files-which-are-in-repository
If the file is still displayed in the status, even though it is in the .gitignore, make sure it isn't already tracked.

git rm --cached IshServices/Web.config
If you just want to ignore it locally, you could also make it ignored by the git status:

git update-index --assume-unchanged config.php
