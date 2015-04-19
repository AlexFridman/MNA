clear; clc;

syms x y z x0 y0 z0 t v yyy(x) zzz(x) K1 K2 K3 K4
a = 1;
m = 0.5;
f = a *(1-y*y)/((1+m)*x*x + y*y + 1);

h = 0.2; 
t0 = 0; 
t1 = 10; 
yy = 0; 

n = floor((t1-t0)/h+1);

x0 = t0:h:t1;
x0 = x0.';
y0 = x0;
z0 = x0;
y0(1) = yy;
z0(1) = yy;

y0(2) = y0(1) + h*subs(subs(f, x, x0(1)), y, y0(1));
for i=3:n
    y0(i) = y0(i-1) + h*(3/2*subs(subs(f, x, x0(i-1)), y, y0(i-1))-1/2*subs(subs(f, x, x0(i-2)), y, y0(i-2)));
end;


dy=zeros(2,1);

[x,y]=ode45(@pr8, [t0 t1], yy);


plot(x0,y0,'-g',x,y,'-r');