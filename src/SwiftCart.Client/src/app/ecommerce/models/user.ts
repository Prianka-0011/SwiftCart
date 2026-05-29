export type User = {
  id: string;
  username?: string;
  email: string;
  addresses: Address[];
  role: string;
};

export type Address = {
  line1: string;
  line2?: string;
  city: string;
  state: string;
  country: string;
  postalCode: string;
};
