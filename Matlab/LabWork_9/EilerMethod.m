clear; clc;

syms x y z x0 y0 z0 t v yyy(x) zzz(x) K1 K2 K3 K4
a = 1;
m = 0.5;
f = a *(1-y*y)/((1+m)*x*x + y*y + 1);


h = 0.05; % точность
t0 = 0; % начальное
t1 = 10; % конечное
yy = 0; 

n = floor((t1-t0)/h+1);

x0 = t0:h:t1;
x0 = x0.';
y0 = x0;
z0 = x0;
y0(1) = yy;
z0(1) = yy;

for i=2:n
    y0(i) = y0(i-1) + h*subs(subs(f, x, x0(i-1)+h/2), y, y0(i-1)+h/2 * subs(subs(f,x,x0(i-1)),y,y0(i-1)) );
    
    K1 = subs(subs(f,x,x0(i-1)),y,z0(i-1));                 %
    K2 = subs(subs(f,x,x0(i-1)+h/2),y,z0(i-1)+K1/2);        %
    K3 = subs(subs(f,x,x0(i-1)+h/2),y,z0(i-1)+K2/2);        % Метод Рунге-Кутта
    K4 = subs(subs(f,x,x0(i-1)+h),y,z0(i-1)+K3);            %    
    z0(i) = z0(i-1) + h*(K1 + 2*K2 + 2*K3 + K4)/6;          %    
end;


disp('------------------------------');


plot(x0,y0,'-b',x0,z0,'-g');