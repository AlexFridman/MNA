syms x X y d P q m n
X = 0:0.1:1;
y = [1; 1.41; 1.79; 2.13; 2.46; 2.76; 3.04; 3.3; 3.55; 3.79; 4.01];

for i=1:11
    d(1,i)=y(i);
end;
for i=2:11
    for j=1:12-i
        d(i,j) = d(i-1,j+1) - d(i-1,j);
    end;
end;
d.'
q = x / 0.1;
P = y(1) + q*0;
m = 1 + q*0;
for i=2:10
    m = m * (q - i + 2);
    P = P + m * d(i,1) / factorial(i-1);
end;

pretty(simplify(P))
vpa(subs(P,0.47),5)