1. Project Overview

SwiftCart is a lightweight e-commerce platform designed for small to medium-sized sellers. It allows customers to browse products, add items to a cart, place orders, make payments, and track delivery.
Admin users manage products, categories, orders, inventory, and customers.

2. System Users (Actors)
2.1 Customer

Browse products

Create account

Add items to cart

Make purchases

Track orders

Leave reviews

2.2 Admin

Manage product catalog

Manage inventory

Manage categories

Manage orders

Manage customers

View reports

2.3 Guest User

Browse products

Add to cart

Must register before checkout

3. Functional Requirements
3.1 Authentication & User Management
Customer

Create account (email, password)

Login / Logout

Forgot password, reset password

Manage profile (name, phone, address)

View past orders

Save multiple addresses

Admin

Login to admin portal

Manage employees (optional)

Roles: Admin, Manager (optional future)

3.2 Product Management
Admin Capabilities

Add new product

Edit product (price, stock, category)

Upload multiple images

Manage inventory (stock in/out)

Activate / deactivate product

Set product variations:

Size

Color

Weight

Any attribute

Product Fields

Name

Description

Price

Selling price (discount)

SKU

Stock quantity

Images

Category

Tags

3.3 Category & Subcategory

Admin can create:

Main categories (e.g., Electronics)

Subcategories (e.g., Headphones)

Products assigned to one or many categories

Category should be filterable by:

Price

Rating

Brand

3.4 Shopping Cart
Customer/Guest Capabilities

Add product to cart

Remove product from cart

Update quantity

Validate stock availability

Persist cart after page refresh

Cart saved for logged-in users

3.5 Checkout & Order System
Checkout Steps

Add items to cart

Select address

Select shipping method

Select payment method

Place order

Order Fields

Order number

Customer

Items list

Subtotal

Taxes

Shipping cost

Discount (if any)

Grand total

Payment status

Order status

Tracking number

Order Status Flow

Pending

Confirmed

Packed

Shipped

Out for Delivery

Delivered

Canceled

Returned

3.6 Payment Management

Support for multiple payment methods:

Online

Credit/Debit Card

PayPal / Stripe

UPI (optional)

Offline

Cash on Delivery (COD)

Requirements

Mark payment status as: Paid, Pending, Failed

Store transaction ID

Generate invoice

3.7 Shipping & Address Management

Customers can:

Add multiple addresses

Choose default address

Track shipments

View estimated delivery dates

Admin can:

Mark orders as shipped

Enter courier details

Update tracking number

3.8 Product Search & Filters
Search

Product name

Description

Tags

Filters

Category

Subcategory

Price range

Brand

Rating

In stock only

Sorting

Popularity

Newest

Price low → high

Price high → low

3.9 Customer Reviews & Ratings

Customers can:

Leave rating (1–5)

Leave written reviews

Upload review photos

Edit or delete their review

Admin can:

Moderate reviews

Hide inappropriate reviews

3.10 Wishlist

Customers can:

Save products to wishlist

Move wishlisted items to cart

3.11 Admin Dashboard

Admins can view:

Sales KPIs

Total revenue

Total orders

Total customers

Total products sold

Graphs

Sales by day/week/month

Top-selling products

Low-stock alerts

Management modules

Product list

Category list

Order list

Customer list

3.12 Notifications
Customer Notifications

Order confirmation

Order shipped

Order delivered

Password reset

Admin Notifications

Low inventory

Bulk orders

High demand products

4. Non-Functional Requirements
4.1 Performance

Pages load under 2 seconds

Support at least 1000 concurrent users

Images optimized

4.2 Security

JWT authentication

Password hashing

Admin protected routes

SQL injection protection

Role-based authorization

4.3 Scalability

Microservice ready architecture

Modular domain design

Can scale product catalog easily

4.4 Reliability

99% uptime

Retry mechanism for failed payments

5. Technical Requirements (Clean Architecture)
Backend

.NET 8 Web API

EF Core

Clean Architecture (Domain → Application → Infrastructure → API)

MediatR (CQRS)

AutoMapper

SQL Server

Frontend

Angular + Material

State management (NgRx optional)

DevOps

Docker support

CI/CD pipelines (GitHub Actions optional)

6. Future Enhancements

Coupon codes

Loyalty points

Multi-vendor marketplace

Notification center (in-app)

AI product recommendation

Purchase history analytics

Inventory forecasting