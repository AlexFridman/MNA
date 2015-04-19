function dy=pr8(x,y)

a = 1;
m = 0.5;
dy = a *(1-y*y)/((1+m)*x*x + y*y + 1);
end