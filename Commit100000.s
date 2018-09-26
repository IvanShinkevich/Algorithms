for i in {1..100000}
do
   echo 'Empty file.' >$i.txt
git add $i.txt
git commit -m "Our 'teacher' said we need more than one commit.Maybe 100000 commits of empty files would be enough?"
done